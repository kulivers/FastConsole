using Comindware.TeamNetwork.Core.KafkaServices;
using Confluent.Kafka;
using Newtonsoft.Json;

class Program
{
    private static IConsumer<int, BrokerMessage> GetConsumer(AutoOffsetReset? autoOffsetReset = null)
    {
        var consumerConfig = new KafkaConfig().DefaultConsumerConfig;
        consumerConfig.GroupId = "adsadad23";
        if (autoOffsetReset != null)
        {
            consumerConfig.AutoOffsetReset = autoOffsetReset;
        }

        var consumerBuilder = new ConsumerBuilder<int, BrokerMessage>(consumerConfig).SetValueDeserializer(new BrokerMessageSerializer());
        return consumerBuilder.Build();
    }

    public const string Topic = "coolTopic2";

    public static void Main(string[] args)
    {
        var total = 0;
        var consumers = Enumerable.Range(0, 20).Select(_ => GetConsumer()).ToArray();

        var consumer = GetConsumer(AutoOffsetReset.Earliest);
        consumer.Subscribe(Topic);
        // while (true)
        // {
        //     var consumeResult = consumer.Consume(1000);
        //     if (consumeResult == null)
        //     {
        //         Console.WriteLine($"{++total} sec");
        //         continue;
        //     }
        //     var data = consumeResult.Message.Value.Data;
        //     var json = JsonConvert.SerializeObject(data);
        //     Console.WriteLine(json);
        //     Console.WriteLine("created new consumer");
        //     // consumer = GetConsumer(AutoOffsetReset.Earliest);
        //     // consumer.Subscribe(Topic);
        // }
        Parallel.ForEach(consumers, consumer =>
        {
            consumer.Subscribe(Topic);
            while (true)
            {
                consumer.Consume(CancellationToken.None);
                Console.WriteLine($"{consumer.Name} - {total}");
                Interlocked.Increment(ref total);
            }
        });
        Console.WriteLine(total);
    }
}