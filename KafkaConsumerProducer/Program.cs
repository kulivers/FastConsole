using System.IO;
using Comindware.TeamNetwork.Core.KafkaServices;
using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Program
{
    public static int I = 0;
    public const string ErrorsTopic = "errorsTopic";

    private static long GetTotalFreeSpace(string driveName)
    {
        foreach (DriveInfo drive in DriveInfo.GetDrives())
        {
            if (drive.IsReady && drive.Name == driveName)
            {
                return drive.TotalFreeSpace;
            }
        }

        return -1;
    }

    public static async Task Main(string[] args)
    {
        // IntBomber();
        NullBomber(20, 1, false);
    }

    private static void IntBomber()
    {
        for (int i = 0; i < 50; i++)
        {
            var i1 = i;
            Task.Run(async () =>
            {
                BrokerMessage message = new BrokerMessage() { Data = Convert(GetMock()) };
                var producer = GetIntProducer();
                while (true)
                {
                    var message1 = new Message<int, BrokerMessage>() { Value = message, Key = i1 };
                    producer.Produce(Topic, message1);
                    Console.WriteLine($"{producer.Name} - {++I} - key: {i1}");
                    // await Task.Delay(new Random().Next(0, 10) * 1000);
                }
            });
        }

        Console.ReadLine();
    }

    private static void NullBomber(int countofProducers, int secAwait, bool asParallel)
    {
        if (asParallel)
        {
            for (var i = 0; i < countofProducers; i++)
            {
                Task.Run(async () =>
                {
                    BrokerMessage message = new BrokerMessage() { Data = Convert(GetMock()) };
                    var producer = GetNullProducer();
                    while (true)
                    {
                        var message1 = new Message<Null, BrokerMessage>() { Value = message };
                        producer.Produce(Topic, message1);
                        Console.WriteLine($"{producer.Name} - {++I}");
                        await Task.Delay(secAwait * 1000);
                    }
                });
            }

            Console.ReadLine();
        }
        else
        {
            var message = new BrokerMessage() { Data = Convert(GetMock()) };
            var producer = GetNullProducer();
            while (true)
            {
                var message1 = new Message<Null, BrokerMessage>() { Value = message };
                producer.Produce(Topic, message1);
                Console.WriteLine($"{producer.Name} - {++I}");
                Thread.Sleep(secAwait * 1000);
            }
        }
    }

    private static IProducer<int, BrokerMessage> GetIntProducer()
    {
        var defaultProducerConfig = new KafkaConfig().DefaultProducerConfig;
        var producer = new ProducerBuilder<int, BrokerMessage>(defaultProducerConfig)
                       .SetValueSerializer(new BrokerMessageSerializer()).Build();
        return producer;
    }

    private static IProducer<Null, BrokerMessage> GetNullProducer()
    {
        ProducerConfig defaultProducerConfig = new KafkaConfig().DefaultProducerConfig;
        IProducer<Null, BrokerMessage> producer = new ProducerBuilder<Null, BrokerMessage>(defaultProducerConfig)
                                                  .SetValueSerializer(new BrokerMessageSerializer()).Build();
        return producer;
    }

    private static IConsumer<Null, BrokerMessage> GetConsumer()
    {
        var consumerConfig = new KafkaConfig().DefaultConsumerConfig;
        consumerConfig.AutoOffsetReset = AutoOffsetReset.Latest;
        var consumerBuilder = new ConsumerBuilder<Null, BrokerMessage>(consumerConfig).SetValueDeserializer(new BrokerMessageSerializer());
        return consumerBuilder.Build();
    }

    private static IConsumer<Null, string> GetStringConsumer()
    {
        var consumerConfig = new KafkaConfig().DefaultConsumerConfig;
        consumerConfig.AutoOffsetReset = AutoOffsetReset.Latest;
        var consumerBuilder = new ConsumerBuilder<Null, string>(consumerConfig).SetValueDeserializer(new StringDeserializer());
        return consumerBuilder.Build();
    }

    public const string Topic = "coolTopic";

    public static ElasticQueryDto GetMock()
    {
        var creds = new BasicCredentials("admin", "C0m1ndw4r3Pl@tf0rm");
        return new ElasticQueryDto(creds, HttpMethod.Put, "/cmw_developdev25_sln.1_event_aa.3/_doc/08dac95d-b139-e4bb-0000-000000000002", data);
        return new ElasticQueryDto(creds, HttpMethod.Put, new Uri("http://localhost:9200/"), data,
            "cmw_developdev25_sln.1_event_aa.3", "_doc", Guid.Parse("08dac95d-b139-e4bb-0000-000000000002"));
    }

    private const string data =
        "{\n  \"id\": \"08dac95d-b139-e4bb-0000-000000000002\",\n  \"origin_event\": \"08dac95d-b139-e4bb-0000-000000000002\",\n  \"time\": \"2022-11-18T12:16:29.7783502Z\",\n  \"initiator\": {\n    \"abbreviation\": \"ad\",\n    \"type\": \"Account\",\n    \"id\": \"account.1\",\n    \"name\": \"admin\",\n    \"alias\": \"admin\"\n  },\n  \"type\": \"UserCommandStarted\",\n  \"status\": \"Success\",\n  \"solution\": {\n    \"type\": \"Solution\",\n    \"id\": \"sln.1\",\n    \"name\": \"Первое приложение\",\n    \"alias\": \"systemSolution\"\n  },\n  \"container\": {\n    \"type\": \"Container\",\n    \"id\": \"aa.3\",\n    \"name\": \"yyyyy\",\n    \"alias\": \"yyyyy\"\n  },\n  \"object\": {\n    \"type\": \"UserCommand\",\n    \"id\": \"event.23\",\n    \"name\": \"Сохранить\",\n    \"alias\": \"edit\"\n  },\n  \"context_objects\": [\n    \"account.3\"\n  ]\n}";

    public static Dictionary<string, object> Convert(ElasticQueryDto dto)
    {
        return new Dictionary<string, object>
        {
            ["Headers"] = dto.Headers,
            ["Method"] = dto.Method,
            ["Address"] = dto.Address,
            ["Data"] = dto.Data,
        };
    }
}