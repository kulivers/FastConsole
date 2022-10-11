using Avro;
using Avro.Reflect;
using Microsoft.Hadoop.Avro;
using Microsoft.Hadoop.Avro.Container;
using Newtonsoft.Json;
using SolTechnology.Avro;

namespace FastConsole;

internal class SerializationDemo
{
    public void RunReflectDemo()
    {
        var employee = Helper.CreateEmployee();
        var path = "D:\\Playground\\Garbage\\FastConsole\\FastConsole\\AvroDemo\\writtenSchema.json";
        var schemaJson = File.ReadAllText(path);
        var schema = Schema.Parse(schemaJson);
        
        var reflectWriter = new ReflectWriter<EmployeeDTO>(schema);
        byte[] spam = new byte[10000];
        using (var ms = new MemoryStream(spam))
        {
            var binaryEncoder = new Avro.IO.BinaryEncoder();
            reflectWriter.Write(employee, binaryEncoder);
        }
    }
   public void RunDefaultDemoSuccess()
    {
        Console.WriteLine("Running the Avro serialization demo ...");
        EmployeeDTO employee = Helper.CreateEmployee();
        
        var bytes = SerializerAvro.Serialize(employee);
        Console.WriteLine("Serialized object to {0} bytes", bytes.Length);
        Console.WriteLine("Bytes are: {0}", BitConverter.ToString(bytes));

        
        Console.WriteLine("Deserializing bytes back into object ... " + Environment.NewLine);
        var regenerated = SerializerAvro.Deserialize<EmployeeDTO>(bytes);
        AreEqual(employee, regenerated);
    }

   public void RunDefaultDemoFailture()
   {
       var valueTuples = new List<Wrapped>() {new Wrapped("a"), new Wrapped("b")}.ToArray();
       var predicateStatement = new PredicateStatement(new Predicate("sdada"), valueTuples);

       var wrapped = valueTuples.First();
       var bytes = SerializerAvro.Serialize<PredicateStatement>(predicateStatement);
       var regenerated = SerializerAvro.Deserialize<PredicateStatement>(bytes);
   }

   public void RunHadoopDemo2()
   {
       var valueTuples = new List<Wrapped>() { new Wrapped(), new Wrapped() };
       var predicate = new Predicate("asda");
       var predicateStatement = new PredicateStatement(predicate, valueTuples);
       var statements = new List<PredicateStatement>();
       WorkAvroHadoob(statements);
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
   
   public static void WorkAvroHadoob(List<PredicateStatement> statements)
   {
       string line = Environment.NewLine;

       string fileName = "Messages.avro";
       string filePath = null;
       if (Environment.NewLine.Contains("\r"))
       {
           filePath = new DirectoryInfo(".") + @"\" + fileName;
       }
       else
       {
           filePath = new DirectoryInfo(".") + @"/" + fileName;
       }

       using (var dataStream = new FileStream(filePath, FileMode.Create))
       {
           using (var avroWriter = AvroContainer.CreateWriter<PredicateStatement>(dataStream, Codec.Deflate))
           {
               using (var seqWriter = new SequentialWriter<PredicateStatement>(avroWriter, statements.Count))
               {
                   statements.ForEach(seqWriter.Write);
               }
           }

           dataStream.Dispose();
       }
   }
}