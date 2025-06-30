# 🩺 Physician Note Extraction – Synapse Technical Assessment

This project is a full-stack implementation that extracts structured information from unstructured physician notes using a hybrid approach:

- A **FastAPI backend** (Python) that uses a Named Entity Recognition (NER) model and regular expressions to parse key data.
- A **C# console application** that reads `.txt` files, sends them to the backend for processing, and captures the results in a JSON file.

---

## 🧠 Features

- ✅ Uses a custom-trained or preloaded NER model for medical device order extraction.
- ✅ Applies pattern-matching via regular expressions for additional fields.
- ✅ Supports multithreaded file processing in C#.
- ✅ Automatically logs progress and responses.
- ✅ Easily configurable via `appsettings.json`.

---

## 🛠 Tools Used

| Tool/Framework      | Purpose                            |
|---------------------|------------------------------------|
| Visual Studio 2022  | C# development                     |
| VS code for Python and FastAPI 
|FastAPI (Python)    | Backend API for NER & regex        |
| .NET 7.0            | Console app + HttpClient           |
| xUnit & Moq         | Unit testing in C#                 |
| GitHub Copilot Chat | AI-assisted refactoring & testing  |

> AI tools were used to create readme file and help with Moq for Xunit

---

## 📂 Configuration

In `appsettings.json`, define your endpoint:

```json
{
  "ApiSettings": {
    "EndpointUrl": "http://localhost:8000/extract"
  }
}

```
You may also use a mock or test endpoint like https://alert-api.com/DrExtract to simulate failures. (Note: this fake endpoint will throw errors because it’s not real.)

🚀 Running the Project
1. Start the FastAPI Server
  Install FastAPI and Uvicorn:
  
```pip install fastapi uvicorn
```
  tart the backend:
 
``` uvicorn app:app --reload
```
  Replace app:app with your module path if it's different.
2. Run the C# Processor
  - Drop .txt files into a /Notes/ folder at the project root.
  - From Visual Studio: set the C# project as startup and run with F5.
  - Or via terminal:
  dotnet build
  dotnet run --project ExtractPhysicianNote

  dotnet build
  dotnet run --project ExtractPhysicianNote
  Results will be posted to the API and optionally saved to JSON.

✅ Unit Testing
  Navigate to the test project and run:
  dotnet test


T ests mock HTTP responses and assert payload/output behavior using xUnit and Moq.

🧹 Recommendations





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
- Write processed results to a local results archive or database
- Add logic to clean/reset the output JSON file once all files are processed.
- API result validation against a schema
- Real-time file monitoring for hot processing


