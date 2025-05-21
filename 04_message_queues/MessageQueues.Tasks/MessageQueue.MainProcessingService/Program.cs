using MessageQueue.MainProcessingService.Listener;

class Program
{
    static async Task Main(string[] args)
    {
        await MessageQueueListener.ListenQueueAsync();
        Console.ReadLine();
    }
}