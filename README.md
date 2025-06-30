# Synapse Technical Assessment – Physician Note Extractor

This project processes physician note text files and sends them to a FastAPI endpoint for medical device order extraction. It includes a C# console application and a corresponding Python API.

---

## 🧰 Tools & IDEs Used

- **IDE:** Visual Studio 2022 for C# code and VS code for Python code and FastAPI
- **.NET SDK:** .NET 7.0
- **Python API:** FastAPI 0.110+
- **Test Framework:** xUnit, Moq

---

## 🤖 AI Development Tools Used

- **Microsoft Copilot (AI assistant)** was used to:
  - Generate Moq
  - Create this `README.md` for clarity and completeness
---

## 📝 Assumptions, Limitations, and Future Improvements

### Assumptions

- Each `.txt` file represents one physician note
- The FastAPI endpoint accepts a POST request with a JSON payload: `{ "text": "..." }`
- A valid `appsettings.json` file exists with `ApiSettings:EndpointUrl`

### Limitations

- No retry logic on failed HTTP requests
- API response is logged but not persisted or validated
- File processing is synchronous and single-threaded

### Future Improvements

- Add JSON schema validation for response structure
- Handle parallel file processing with throttling
- Integrate Serilog for structured logging
- Write processed results to a local results archive (e.g., `results.json`)

---

## 🚀 How to Run This Project

### 1. Start the Python FastAPI Server

Make sure you have Python and Uvicorn installed:

```bash or vs code terminal

pip install fastapi uvicorn
run uvicorn app:app --reload 

