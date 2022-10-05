using Avro;
using Avro.Generic;
using Avro.IO;
using Avro.Reflect;
using Avro.Specific;
using FastConsole;
using SolTechnology.Avro;


internal class Program2
{
    public void LocalMain2()
    {
        var serDemo = new SerializationDemo();
        serDemo.Run();
    }
    public void LocalMain()
    {
        var person = new Person(323212, "ega");
        var json = AvroConvert.GenerateSchema(typeof(Person));
        var schema = Schema.Parse(json);
        

        DatumWriter<Person> writer = new ReflectWriter<Person>(schema);
        DatumReader<Person> reader = new GenericReader<Person>(schema, schema);
        
        var span = new Span<byte>();
        using (var ms = new MemoryStream())
        {
            
            writer.Write(person, new BinaryEncoder(ms));
            span = ms.ToArray();
        }

      
        
        using (var ms = new MemoryStream(span.ToArray()))
        {
            var person1 = new Person();
            var read = reader.Read(person1, new BinaryDecoder(ms));

            Console.WriteLine(person.Equals(person1) ? "Success!!!!" : "Failture");
        }
    }

    public static byte[] ReadFully(Stream input)
    {
        byte[] buffer = new byte[16 * 1024];
        using (MemoryStream ms = new MemoryStream())
        {
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }

            return ms.ToArray();
        }
    }
}

class Program
{
    public static async Task Main(string[] args)
    {
        var proga = new Program2();
        proga.LocalMain2();
    }
}