using MessageQueue.DataCaptureService.Services;

class Program
{
    private static readonly string fileFormat = "*.txt";

    static void Main(string[] args)
    {
        FileService.ListenFolderAndSendMessage(fileFormat);
        Console.ReadLine();
    }
}