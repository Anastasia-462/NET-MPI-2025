using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using MessageQueue.MainProcessingService.Service;
using MessageQueue.MainProcessingService.Service.Configuration;

namespace MessageQueue.MainProcessingService.Listener
{
    public static class MessageQueueListener
    {
        public static async Task ListenQueueAsync()
        {
            var config = ConfigurationService.GetConfiguration();
            var factory = new ConnectionFactory();
            factory.Uri = new Uri(config.Uri);

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(config.QueueName, true, false, false);
            await channel.QueueBindAsync(config.QueueName, config.ExchangeName, string.Empty);
            
            var consumer = new AsyncEventingBasicConsumer(channel);
            var longMessages = new List<MessageObject>();
            consumer.ReceivedAsync += async (sender, eventArgs) =>
            {
                var headers = eventArgs.BasicProperties.Headers;
                var fileName = Encoding.UTF8.GetString((byte[])headers["file_name"]);
                var position = (int)headers["position"];
                var length = (int)headers["length"];
                var size = (int)headers["size"];

                if (position == 0)
                {
                    FileService.SaveFile(fileName, eventArgs.Body.ToArray());
                }
                else
                {
                    longMessages.Add(new MessageObject
                    {
                        Size = size,
                        Length = length,
                        FileName = fileName,
                        Position = position,
                        Message = eventArgs.Body.ToArray()
                    });

                    var msgList = longMessages.Where(x => x.FileName == fileName).ToList();
                    if (msgList.Sum(x => x.Size) == length)
                    {
                        var message = msgList.OrderBy(x => x.Position).SelectMany(x => x.Message).ToArray();
                        FileService.SaveFile(fileName, message);
                    }
                }
            };

            await channel.BasicConsumeAsync(config.QueueName, true, consumer);
        }

        private class MessageObject
        {
            public int Position { get; set; }

            public string FileName { get; set; }

            public int Length { get; set; }

            public int Size { get; set; }

            public byte[] Message { get; set; }
        }
    }
}
