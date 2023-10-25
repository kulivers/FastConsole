using System.IO;
using Comindware.TeamNetwork.Core.KafkaServices;
using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DateTime = System.DateTime;

class Program
{
    public const string Topic = "coolTopic";

    public static ElasticQueryDto GetMock()
    {
        return new ElasticQueryDto(new BasicCredentials("admin", "C0m1ndw4r3Pl@tf0rm"), HttpMethod.Put, new Uri("http://localhost:9200"), data,
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

    private static IProducer<int, BrokerMessage> GetIntProducer()
    {
        var defaultProducerConfig = new KafkaConfig().DefaultProducerConfig;
        var producer = new ProducerBuilder<int, BrokerMessage>(defaultProducerConfig)
            .SetValueSerializer(new BrokerMessageSerializer()).Build();
        return producer;
    }
    private static IProducer<Null, BrokerMessage> GetNullProducer()
    {
        var defaultProducerConfig = new KafkaConfig().DefaultProducerConfig;
        var producer = new ProducerBuilder<Null, BrokerMessage>(defaultProducerConfig)
            .SetValueSerializer(new BrokerMessageSerializer()).Build();
        return producer;
    }

    public static void Main(string[] args)
    {
        
        
        var total = 9;
        var mock = Convert(GetMock());
        var producers = Enumerable.Range(0, 10).Select(_ => GetIntProducer()).ToArray();
        var producer = GetNullProducer();
        while (true)
        {
            var data = new Dictionary<string, object>() { { "total", ++total } };
            var timestamp = new Timestamp(new DateTime(2022, 12, 1));
            var message = new Message<Null, BrokerMessage>() { Value = new BrokerMessage() { Data = data }, Timestamp = timestamp };
            producer.Produce(Topic, message);
        }

        return;

        Parallel.For(0, 10, ((i, state) =>
        {
            var producer = producers[i];
            var message = new Message<int, BrokerMessage>() { Key = i, Value = new BrokerMessage() { Data = mock } };
            while (true)
            {
                producer.Produce(Topic, message);
                Console.WriteLine($"{producer.Name} - {++total}");
            }
        }));
        Console.ReadLine();
    }
}