using Common;
using System.Net;
using System.Text;

namespace Synapse.Technical_Assessment
{
    class ExtractPhysicianNote
    {
        static async Task Main(string[] args)
        {
            try
            {
                //get the API endpoint from configuration
                string endpointUrl = AppUtilities.LoadApiEndpoint("ApiSettings:ExtractUrl");
                //get the folder path from project root
                string? folderPath = GetProjectRootFolder("Notes");

                if (!Directory.Exists(folderPath))
                {
                    AppUtilities.LogError($"Folder not found: {folderPath}");
                    return;
                }
                //get all .txt files in the folder
                string[] files = Directory.GetFiles(folderPath, "*.txt");
                AppUtilities.LogInfo($"Found {files.Length} .txt files to process");

                foreach (string filePath in files)
                {
                    try
                    {
                        //process each file
                        await ProcessFileAsync(filePath, endpointUrl);
                    }
                    catch (Exception ex)
                    {
                        AppUtilities.LogError($"Error processing file {Path.GetFileName(filePath)}: {ex.Message}");
                    }
                    await SendJsonFileToAPI();

                }

            }
            catch (Exception ex)
            {
                AppUtilities.LogError($"Unhandled application error: {ex.Message}");
            }
        }



        static string? GetProjectRootFolder(string subfolder)
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, subfolder)))
            {
                dir = dir.Parent;
            }
            return dir != null ? Path.Combine(dir.FullName, subfolder) : null;
        }

        static async Task ProcessFileAsync(string filePath, string endpointUrl)
        {
            AppUtilities.LogInfo($"Starting: {Path.GetFileName(filePath)}");

            string fileContent = await File.ReadAllTextAsync(filePath);
            var payload = new { text = fileContent };

            using var client = new HttpClient();

            var response = await APICall.SendTextToApiAsync(client, endpointUrl, payload.text);

            AppUtilities.LogInfo($"Completed: {Path.GetFileName(filePath)} | Response: {response}");
        }
        //note this method is used to send the json file to the API endpoint
        //it will throw an error if the file is not found or if the API endpoint is not reachable
        static async Task<HttpStatusCode> SendJsonFileToAPI()
        {
            try
            {
                string jsonFilePath =Path.Combine( GetProjectRootFolder(AppUtilities.LoadApiEndpoint("ApiSettings:JsonFolder")), "dme_results.json"); 
                string apiUrl = AppUtilities.LoadApiEndpoint("ApiSettings:EndpointUrl");  

                // Read and prepare the JSON
                string jsonData = await File.ReadAllTextAsync(jsonFilePath);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                using var client = new HttpClient();
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                // Output the result
                string result = await response.Content.ReadAsStringAsync();
                AppUtilities.LogInfo($"✅ Status: {response.StatusCode}");
                AppUtilities.LogInfo($"📨 Response: {result}");
                 return HttpStatusCode.OK;

            }
            catch (Exception ex)
            {
                AppUtilities.LogError($"❌ Error: {ex.Message}");
                return HttpStatusCode.BadRequest;
            }
          
        }
    }
}