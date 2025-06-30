
from transformers import pipeline, AutoTokenizer, AutoModelForTokenClassification
import re
import json
import os
from pathlib import Path

# Load NER model and tokenizer
model_id = "d4data/biomedical-ner-all"

tokenizer = AutoTokenizer.from_pretrained(model_id)
model = AutoModelForTokenClassification.from_pretrained(model_id)

ner_pipeline = pipeline("ner", model=model, tokenizer=tokenizer, aggregation_strategy="max")

def clean_entity(entities, target_label):
    return [
        e["word"] for e in entities if e["entity_group"] == target_label
    ]


    
def extract_payload(text):
    try:
        json_obj = json.loads(text)
        if "data" in json_obj:
            return json_obj["data"]          
    except json.JSONDecodeError:
        pass  # Not JSON, treat as plain text
    return text

def extract_diagnosis(text, entities):
    diagnosis_ner = clean_entity(entities, "Disease_disorder")
    if diagnosis_ner:
        return " ".join(diagnosis_ner).strip()

    match = re.search(r"Diagnosis:\s*(.+)", text, re.I)
    if match:
        return match.group(1).strip()

    return None
    
def save_results(results, json_path):
    json_path = Path(json_path)
    json_path.parent.mkdir(parents=True, exist_ok=True)

    # Load existing data if present
    try:
        with json_path.open("r", encoding="utf-8") as f:
            existing_data = json.load(f)
    except (FileNotFoundError, json.JSONDecodeError):
        existing_data = []

    # Normalize both to lists and merge
    if not isinstance(existing_data, list):
        existing_data = [existing_data]
    results = results if isinstance(results, list) else [results]

    # Write updated data
    with json_path.open("w", encoding="utf-8") as f:
        json.dump(existing_data + results, f, indent=2)

            
def extract_dme_fields(text):
    result = {
        "patient_name": None,
        "dob": None,
        "diagnosis": None,
        "device": None,
        "device_description": None,
        "liters": None,
        "usage": None,
        "ahi": None,
        "ordering_provider": None
    }

    # Step 1: Run NER
    entities = ner_pipeline(text)

    result["diagnosis"] = extract_diagnosis(text, entities)
    procedures = clean_entity(entities, "Therapeutic_procedure")
    
    if any("oxygen" in p.lower() for p in procedures):
        result["device"] = "Oxygen Tank"
    elif any("cpap" in p.lower() for p in procedures):
        result["device"] = "CPAP"

    # Fallback extraction with regex
    def match(pattern):
        return re.search(pattern, text, re.I)

    if m := match(r"Patient Name:\s*(.+)"):
        result["patient_name"] = m.group(1).strip()
    if m := match(r"DOB:\s*([\d/]+)"):
        result["dob"] = m.group(1).strip()
    if m := match(r"Ordering Physician:\s*(.+)"):
        result["ordering_provider"] = m.group(1).strip()
    if m := match(r"Usage:\s*(.+)"):
        result["usage"] = m.group(1).strip()
    if m := match(r"(\d+\s?L(?: per minute)?)"):
        result["liters"] = m.group(1).strip()
    if m := match(r"Recommendation:\s*(.+)"):
        result["device_description"] = m.group(1).strip()
    if m := match(r"AHI:\s*(\d+)"):
         result["ahi"] = m.group(1).strip()
    return result
