using System.Net;
using Confluent.Kafka;

public class KafkaConfig
{
    private const string BootstrapServers = "192.168.0.127:9092";
    private const string DefaultGroupId = "foo";

    public ProducerConfig DefaultProducerConfig =>
        new()
        {
            BootstrapServers = BootstrapServers,
            ClientId = Dns.GetHostName(),
            Partitioner = Partitioner.Consistent,
            Acks = Acks.All,
        };

    public ConsumerConfig DefaultConsumerConfig =>
        new()
        {
            BootstrapServers = BootstrapServers,
            GroupId = DefaultGroupId,
            AutoOffsetReset = AutoOffsetReset.Latest,
        };
}