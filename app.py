from fastapi import FastAPI, Request
from pydantic import BaseModel
from extractor import extract_dme_fields, save_results,extract_payload
import json

app = FastAPI()

with open("config.json") as f:
    config = json.load(f)
output_json = config["output_path"]

class NoteInput(BaseModel):
    text: str

@app.post("/extract")
def extract_fields(payload: NoteInput):

    json = extract_payload(payload.text)  
    result = extract_dme_fields(json)
    save_results(result, output_json)
    print(f"✅ Extraction complete! JSON saved to {output_json}")
    return {"result": result}

