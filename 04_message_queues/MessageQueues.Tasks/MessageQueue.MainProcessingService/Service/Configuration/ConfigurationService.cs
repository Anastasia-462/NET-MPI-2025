using System.Text.Json;

namespace MessageQueue.MainProcessingService.Service.Configuration
{
    public static class ConfigurationService
    {
        public static Configuration GetConfiguration()
        {
            try
            {
                string filePath = "appconfig.json";

                string directory = new DirectoryInfo(Environment.CurrentDirectory)?.Parent?.Parent?.Parent?.FullName ?? string.Empty;
                string path = directory + @"\" + filePath;

                if (!File.Exists(path))
                {
                    Console.WriteLine("File doesn't exist.");
                    return new Configuration();
                }

                string jsonContent = File.ReadAllText(path);
                var config = JsonSerializer.Deserialize<Configuration>(jsonContent);

                return config;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return new Configuration();
            }
        }
    }
}
