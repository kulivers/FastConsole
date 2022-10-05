using Microsoft.Hadoop.Avro;
using Newtonsoft.Json;
using SolTechnology.Avro;

namespace FastConsole;

internal class SerializationDemo
{
    public void RunDefaultDemo()
    {
        Console.WriteLine("Running the Avro serialization demo ...");

        var employee = Helper.CreateEmployee();


        // Serialization
        var bytes = SerializerAvro.Serialize(employee);
        Console.WriteLine("Serialized object to {0} bytes", bytes.Length);
        Console.WriteLine("Bytes are: {0}", BitConverter.ToString(bytes));

        // Deserialization
        Console.WriteLine("Deserializing bytes back into object ... " + Environment.NewLine);
        var regenerated = SerializerAvro.Deserialize<EmployeeDTO>(bytes);

        // Verification : We compare original object with the object regenerated
        // after passing through a serialize=>deserialize round trip. 
        // We compare the Json equivalent of this object to keep it simple and lazy
        // and don't feel like implementing a proper 'Equals' method
        // FYI, Json usage here has NOTHING to do with Avro serialization 
        AreEqual(employee, regenerated);
    }

    private static void AreEqual(object employee, object regenerated)
    {
        var origJson = JsonConvert.SerializeObject(employee);
        var regenJson = JsonConvert.SerializeObject(regenerated);

        if (origJson.Equals(regenJson))
            Console.WriteLine("Success. Object through the serialize=>deserialize round trip are identical.");
        else
            Console.WriteLine("FAILED! We lost data during the serialize=>deserialize round trip");

        //Console.WriteLine(Environment.NewLine + "Press any key to exit ...");
        //Console.ReadLine();
        Console.WriteLine(Helper.HorizontalLine);
    }

    public void RunHadoopDemo()
    {
        Console.WriteLine("Running the Avro serialization demo ...");

        EmployeeDTO expected = Helper.CreateEmployee();

        var avroSerializer = AvroSerializer.Create<EmployeeDTO>();
        using (var buffer = new MemoryStream())
        {
            //Create a data set by using sample class and struct
            
            //Serialize the data to the specified stream
            avroSerializer.Serialize(buffer, expected);


            Console.WriteLine("Deserializing Sample Data Set...");

            //Prepare the stream for deserializing the data
            buffer.Seek(0, SeekOrigin.Begin);

            //Deserialize data from the stream and cast it to the same type used for serialization
            var actual = avroSerializer.Deserialize(buffer);

            AreEqual(expected, actual);

        }
    }
    
}