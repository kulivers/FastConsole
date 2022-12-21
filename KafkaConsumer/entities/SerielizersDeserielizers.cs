using System.Text;
using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Comindware.TeamNetwork.Core.KafkaServices;

internal class BrokerMessageSerializer : ISerializer<BrokerMessage>, IDeserializer<BrokerMessage>
{
    private static readonly JsonSerializerSettings _serializationSettings = new JsonSerializerSettings
    {
        NullValueHandling = NullValueHandling.Ignore,
        DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
        DateFormatHandling = DateFormatHandling.IsoDateFormat,
        DateTimeZoneHandling = DateTimeZoneHandling.Local,
        TypeNameHandling = TypeNameHandling.Auto,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        ObjectCreationHandling = ObjectCreationHandling.Replace,
        Converters = new List<JsonConverter>
        {
            new StringEnumConverter()
        }
    };

    private static JsonSerializer _commonSerializer = JsonSerializer.Create(_serializationSettings);

    public byte[] Serialize(BrokerMessage data, SerializationContext context)
    {
        using var stream = new MemoryStream();
        var jsonWriter = new JsonTextWriter(new StreamWriter(stream));
        _commonSerializer.Serialize(jsonWriter, data);

        jsonWriter.Flush();
        stream.Flush();
        return stream.ToArray();
    }
    public BrokerMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
        {
            return null;
        }
        using (JsonReader reader = new JsonTextReader(new StreamReader(new MemoryStream(data.ToArray()))))
        {
            return _commonSerializer.Deserialize<BrokerMessage>(reader);
        }
    }

}

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
public class StringDeserializer : IDeserializer<string>
{
    public string Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        return Encoding.UTF8.GetString(data);
    }
}