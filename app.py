from fastapi import FastAPI, Request
from pydantic import BaseModel
from extractor import extract_dme_fields, save_results,extract_payload
import json

app = FastAPI()

with open("config.json") as f:
    config = json.load(f)
OUTPUT_JSON = config["output_path"]

class NoteInput(BaseModel):
    text: str

@app.post("/extract")
def extract_fields(payload: NoteInput):

    json = extract_payload(payload.text)  
    result = extract_dme_fields(json)
    save_results(result, OUTPUT_JSON)
    print(f"✅ Extraction complete! JSON saved to {OUTPUT_JSON}")
    return {"result": result}

