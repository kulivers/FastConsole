using Comindware.TeamNetwork.Core.KafkaServices;
using Confluent.Kafka;

public class KafkaProducerFactory
{
    private readonly KafkaConfig _config;

    public KafkaProducerFactory(KafkaConfig config)
    {
        _config = config;
    }

    public IProducer<Null, HttpResponseMessage> GetHttpResponseProducer() =>
        new ProducerBuilder<Null, HttpResponseMessage>(_config.DefaultProducerConfig)
            .SetValueSerializer(new HttpResponseMessageSerializer()).Build();

    public IProducer<Null, HttpResponseMessageWrapper> GetHttpResponseWrapperProducer()
    {
        return new ProducerBuilder<Null, HttpResponseMessageWrapper>(_config.DefaultProducerConfig)
            .SetValueSerializer(new HttpResponseWrapperMessageSerializer()).Build();
    }
}