using Avro.Generic;
using Avro.IO;
using Avro.Specific;

namespace FastConsole;

public static class SerializerAvro
{
    public static byte[] Serialize<T>(T thisObj) where T : ISpecificRecord
    {
        using (var ms = new MemoryStream())
        {
            var enc = new BinaryEncoder(ms);
            var writer = new SpecificDefaultWriter(thisObj.Schema); // Schema comes from pre-compiled, code-gen phase
            writer.Write(thisObj, enc);
            return ms.ToArray();
        }
    }
    
    public static T Deserialize<T>(byte[] bytes) where T : ISpecificRecord, new()
    {
        using (var ms = new MemoryStream(bytes))
        {
            var dec = new BinaryDecoder(ms);
            var regenObj = new T();
            var reader = new SpecificDefaultReader(regenObj.Schema, regenObj.Schema);
            reader.Read(regenObj, dec);
            return regenObj;
        }
    }
    
    public static T DeserializeReuseObject<T>(byte[] bytes, T regenObj) where T : ISpecificRecord
    {
        using (var ms = new MemoryStream(bytes))
        {
            var dec = new BinaryDecoder(ms);

            var reader = new SpecificDefaultReader(regenObj.Schema, regenObj.Schema);
            reader.Read(regenObj, dec);
            return regenObj;
        }
    }
}