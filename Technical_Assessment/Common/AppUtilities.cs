using Microsoft.Extensions.Configuration;

namespace Common
{
    /// <summary>
    /// common methods for logging and configuration management used in both the console application and unit test project.
    /// </summary>
    public static class AppUtilities
    {
        public static void LogInfo(string message) =>
            Console.WriteLine($"[INFO]  {DateTime.Now:HH:mm:ss} | {message}");

        public static void LogError(string message) =>
            Console.WriteLine($"[ERROR] {DateTime.Now:HH:mm:ss} | {message}");

        public static string LoadApiEndpoint(string endPointName)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            return config[endPointName];
        }
    }
}
