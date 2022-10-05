using System.Text;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace Comindware.TeamNetwork.Core.KafkaServices;

public class HttpRequestMessageDeserializer : IDeserializer<HttpRequestMessageWrapper>
{
    public HttpRequestMessageWrapper Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        var json = Encoding.Default.GetString(data.ToArray());
        var deserialized = JsonConvert.DeserializeObject<HttpRequestMessageWrapper>(json);
        return deserialized;
    }
}

public class HttpRequestMessageSerializer : ISerializer<HttpRequestMessage>
{
    public byte[] Serialize(HttpRequestMessage data, SerializationContext context)
    {
        var json = JsonConvert.SerializeObject(data);
        var bytes = Encoding.ASCII.GetBytes(json);
        return bytes;
    }
}

public class HttpResponseMessageSerializer : ISerializer<HttpResponseMessage>
{
    public byte[] Serialize(HttpResponseMessage data, SerializationContext context)
    {
        data.RequestMessage = null;
        var json = JsonConvert.SerializeObject(data);
        var bytes = Encoding.ASCII.GetBytes(json);
        return bytes;
    }
}

public class HttpResponseWrapperMessageSerializer : ISerializer<HttpResponseMessageWrapper>
{
    public byte[] Serialize(HttpResponseMessageWrapper data, SerializationContext context)
    {
        var json = JsonConvert.SerializeObject(data);
        var bytes = Encoding.ASCII.GetBytes(json);
        return bytes;
    }
}

public class HttpResponseMessageDeserializer : IDeserializer<HttpResponseMessage>
{
    public HttpResponseMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        var json = Encoding.ASCII.GetString(data.ToArray());
        var deserialized = JsonConvert.DeserializeObject<HttpResponseMessage>(json);
        return deserialized;
    }
}