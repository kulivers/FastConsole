using Comindware.TeamNetwork.Core.KafkaServices;
using Confluent.Kafka;

public class KafkaConsumerFactory
{
    private readonly KafkaConfig _config;

    public KafkaConsumerFactory(KafkaConfig config)
    {
        _config = config;
    }

    public IConsumer<Null, HttpRequestMessage> GetHttpRequestConsumer()
    {
        return new ConsumerBuilder<Null, HttpRequestMessage>(_config.DefaultConsumerConfig).Build();
    }

    public IConsumer<Ignore, HttpRequestMessageWrapper> GetHttpRequestWrapperConsumer()
    {
        return new ConsumerBuilder<Ignore, HttpRequestMessageWrapper>(_config.DefaultConsumerConfig)
            .SetValueDeserializer(new HttpRequestMessageDeserializer()).Build();
    }
}