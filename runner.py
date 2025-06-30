# this file just for me to test the extractions works correctly
import os
import json
import csv
from extractor import extract_dme_fields,extract_payload

INPUT_FOLDER = "notes" 
OUTPUT_JSON = "output/dme_results.json"


def process_txt_files(folder_path):
    results = []

    for filename in os.listdir(folder_path):
        if filename.endswith(".txt"):
            with open(os.path.join(folder_path, filename), "r", encoding="utf-8") as f:
                
                text = f.read()
                print(f"📄 Processing: {filename}")
                payload = extract_payload(text)  
                extracted = extract_dme_fields(payload)
                results.append(extracted)

    return results

def save_results(results, json_path):
    os.makedirs(os.path.dirname(json_path), exist_ok=True)

    # Save JSON
    with open(json_path, "w") as f_json:
        json.dump(results, f_json, indent=2)


  

if __name__ == "__main__":
    data = process_txt_files(INPUT_FOLDER)
    save_results(data, OUTPUT_JSON)
    print(f"✅ Extraction complete! JSON saved to {OUTPUT_JSON}")
