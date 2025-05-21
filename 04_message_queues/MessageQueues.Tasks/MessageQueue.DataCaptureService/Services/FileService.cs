using MessageQueue.DataCaptureService.Listener;
using MessageQueue.DataCaptureService.Services.Configuration;

namespace MessageQueue.DataCaptureService.Services
{
    public static class FileService
    {
        public static void ListenFolderAndSendMessage(string fileFormat)
        {
            string directory = new DirectoryInfo(Environment.CurrentDirectory)?.Parent?.Parent?.Parent?.FullName ?? string.Empty;
            string folderName = "Local Folder";
            string folderPath = directory + @"\" + folderName;

            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine($"Folder \"{folderName}\" doesn't exist. Completion of the program...");
                return;
            }

            ProcessExistingFiles(folderPath, fileFormat);

            StartWatchingFolder(folderPath, fileFormat);

            Console.WriteLine($"Listening folder: {folderPath}");
            Console.WriteLine("Press Enter to exit...");
        }

        private static void ProcessExistingFiles(string folderPath, string fileFormat)
        {
            string[] existingFiles = Directory.GetFileSystemEntries(folderPath, fileFormat);
            Console.WriteLine($"{existingFiles.Length} existing files were found.");

            foreach (var file in existingFiles)
            {
                ProcessFileAsync(file);
            }
        }

        private static void StartWatchingFolder(string folderPath, string fileFormat)
        {
            var watcher = new FileSystemWatcher
            {
                Path = folderPath,
                Filter = fileFormat,
                EnableRaisingEvents = true,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
            };

            watcher.Created += OnFileAdded;
        }

        private static void OnFileAdded(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Added new file: {e.FullPath}");

            ProcessFileAsync(e.FullPath);
        }

        private static async Task ProcessFileAsync(string filePath)
        {
            try
            {
                var fileContent = File.ReadAllBytes(filePath);
                var fileName = Path.GetFileName(filePath);
                Console.WriteLine($"Processing an existing file: {fileName}");

                await MessageQueueProcessing.SendMessageAsync(ConfigurationService.GetConfiguration(), fileContent, fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File processing error {filePath}: {ex.Message}");
            }
        }
    }
}
