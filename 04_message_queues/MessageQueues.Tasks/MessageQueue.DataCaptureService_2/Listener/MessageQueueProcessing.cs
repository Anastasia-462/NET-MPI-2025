using MessageQueue.DataCaptureService_2.Services.Configuration;
using RabbitMQ.Client;

namespace MessageQueue.DataCaptureService_2.Listener
{
    public static class MessageQueueProcessing
    {
        public static readonly int MaxFrame = 131072;

        public static async Task SendMessageAsync(Configuration config, byte[] fileContent, string fileName)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri(config.Uri);

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();
            try
            {
                await channel.ExchangeDeclareAsync(config.ExchangeName, ExchangeType.Fanout, true);
                await FormMessageAsync(channel, fileContent, fileName, config);
            }
            finally
            {
                if (channel != null)
                    await channel.DisposeAsync();
                if (connection != null)
                    await connection.DisposeAsync();
            }
        }

        private static async Task FormMessageAsync(IChannel channel, byte[] fileContent, string fileName, Configuration config)
        {
            if (fileContent.Length > MaxFrame)
            {
                var size = Math.Ceiling((double)fileContent.Length / MaxFrame);
                var length = fileContent.Length;
                var lastLength = length - (fileContent.Length / MaxFrame) * MaxFrame;
                for (var i = 1; i <= size; i++)
                {
                    var takeContent = i == size ? lastLength : MaxFrame;
                    var properties = new BasicProperties
                    {
                        Headers = new Dictionary<string, object?>
                        {
                            { "file_name", fileName },
                            { "length", fileContent.Length },
                            { "position", i },
                            { "size", takeContent },
                        }
                    };

                    var content = fileContent.Skip((i - 1) * MaxFrame).Take(takeContent).ToArray();
                    await channel.BasicPublishAsync(config.ExchangeName, string.Empty, false, properties, content);
                }
            }
            else
            {

                var properties = new BasicProperties
                {
                    Headers = new Dictionary<string, object?>
                    {
                        { "file_name", fileName },
                        { "length", fileContent.Length },
                        { "position", 0 },
                        { "size", fileContent.Length },
                    }
                };

                await channel.BasicPublishAsync(config.ExchangeName, string.Empty, false, properties, fileContent);
            }
        }
    }
}
