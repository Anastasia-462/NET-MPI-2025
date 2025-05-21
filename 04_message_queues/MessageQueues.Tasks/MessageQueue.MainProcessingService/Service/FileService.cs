namespace MessageQueue.MainProcessingService.Service
{
    public static class FileService
    {
        public static void SaveFile(string fileName, byte[] file)
        {
            string directory = new DirectoryInfo(Environment.CurrentDirectory)?.Parent?.Parent?.Parent?.FullName ?? string.Empty;
            string folderName = "Special Local Folder";
            string folderPath = directory + @"\" + folderName;

            if (!Directory.Exists(folderPath))
            {
                //Directory.CreateDirectory(folderPath);
                Console.WriteLine($"Folder \"{folderName}\" doesn't exist. Completion of the program...");
                return;
            }

            var filePath = Path.Combine(folderPath, fileName);

            File.WriteAllBytes(filePath, file);
            Console.WriteLine($"File '{fileName}' is saved.");
        }
    }
}
