using System.Net;
using Confluent.Kafka;

public class KafkaConfig
{
    private const string BootstrapServers = "hldev04:9092";
    private const string DefaultGroupId = "Comindware";

    public ProducerConfig DefaultProducerConfig =>
        new()
        {
            BootstrapServers = BootstrapServers,
            ClientId = Dns.GetHostName(),
            Partitioner = Partitioner.Consistent,
            Acks = Acks.None
        };

    public ConsumerConfig DefaultConsumerConfig =>
        new()
        {
            BootstrapServers = BootstrapServers,
            GroupId = DefaultGroupId,
            AutoOffsetReset = AutoOffsetReset.Latest
        };
}