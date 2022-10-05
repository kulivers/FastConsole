using System.Runtime.Serialization;
using System.Xml;
using Avro;
using Avro.Generic;
using Avro.IO;
using Avro.Reflect;
using FastConsole;
using Microsoft.Hadoop.Avro.Container;
using SolTechnology.Avro;

internal class Program2
{
    public void LocalMain2()
    {
        var serDemo = new SerializationDemo();
        serDemo.RunHadoopDemo();
    }

    public void LocalMain()
    {
        var fileName = @"D:\Playground\Garbage\FastConsole\FastConsole\writtenSchema.json";

        using (Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            using (var reader = AvroContainer.CreateReader<EmployeeDTO>(stream))
            {
                using (var streamReader = new SequentialReader<EmployeeDTO>(reader))
                {
                    var record = streamReader.Objects.FirstOrDefault();
                }
            }
        }

    }

    public static void WriteObject(string path)
    {
        // Create a new instance of the Person class and
        // serialize it to an XML file.
        var p1 = Helper.CreateEmployee();
        // Create a new instance of a StreamWriter
        // to read and write the data.
        var fs = new FileStream(path, FileMode.Create);
        var writer = XmlDictionaryWriter.CreateTextWriter(fs);
        var ser = new DataContractSerializer(typeof(EmployeeDTO));
        ser.WriteObject(writer, p1);
        Console.WriteLine("Finished writing object.");
        writer.Close();
        fs.Close();
    }
    public static void ReadObject(string path)
    {
        // Deserialize an instance of the Person class
        // from an XML file. First create an instance of the
        // XmlDictionaryReader.
        FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
        XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());

        // Create the DataContractSerializer instance.
        DataContractSerializer ser = new DataContractSerializer(typeof(EmployeeDTO));
        // Deserialize the data and read it from the instance.
        EmployeeDTO newPerson = (EmployeeDTO)ser.ReadObject(reader);
        fs.Close();
    }


    public static byte[] ReadFully(Stream input)
    {
        var buffer = new byte[16 * 1024];
        using (var ms = new MemoryStream())
        {
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0) ms.Write(buffer, 0, read);

            return ms.ToArray();
        }
    }
}

internal class Program
{
    public static async Task Main(string[] args)
    {
        var proga = new Program2();
        proga.LocalMain2();
    }
}