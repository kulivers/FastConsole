using Confluent.Kafka;

public class Program
{
    public static async Task Main(string[] args)
    {
        var bootstrapServers = "192.168.0.127:9092";

        var requesttopic = "requestTopic";
        var topicNames = new[] { "responseTopic", "requestTopic" };

        var admin = new AdminClientBuilder(new KafkaConfig().DefaultProducerConfig).Build();
        var meta = admin.GetMetadata(requesttopic, TimeSpan.FromSeconds(2));

        var consumer = new KafkaConsumerFactory(new KafkaConfig()).GetHttpRequestWrapperConsumer();

        ConsumeHttpRequests(consumer, topicNames);

        void ConsumeHttpRequests(IConsumer<Ignore, HttpRequestMessageWrapper> consumer1, string[] strings)
        {
            var i = 0;
            using (consumer1)
            {
                var currentOffset = Offset.Beginning;
                var topicPartitionOffset = new TopicPartitionOffset(requesttopic, new Partition(0), currentOffset);
                // consumer.Assign(topicPartitionOffset);

                consumer1.Subscribe(strings.First(s => s == "requestTopic"));
                while (true)
                {
                    try
                    {
                        var result = consumer1.Consume(CancellationToken.None);
                        if (result != null)
                        {
                            Console.WriteLine(i++);
                            currentOffset = result.Offset;
                            topicPartitionOffset =
                                new TopicPartitionOffset(requesttopic, new Partition(0), currentOffset);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
        }
    }
}