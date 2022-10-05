using System.Runtime.Serialization;
using Avro;
using Avro.Specific;

namespace FastConsole;

public struct ProjectDTO : ISpecificRecord
{
    public static Schema _SCHEMA = Schema.Parse(
        "{\"type\":\"record\",\"name\":\"ProjectDTO\",\"namespace\":\"FastConsole\",\"fields\":[{\"name" +
        "\":\"ProjectId\",\"type\":\"int\"},{\"name\":\"ProjectName\",\"type\":\"string\"}]}");

    
    public int ProjectId { get; set; }
    

    public string ProjectName { get; set; }

    public Schema Schema => _SCHEMA;

    
    public object Get(int fieldPos)
    {
        switch (fieldPos)
        {
            case 0: return ProjectId;
            case 1: return ProjectName;
            default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
        }
    }

    public void Put(int fieldPos, object fieldValue)
    {
        switch (fieldPos)
        {
            case 0:
                ProjectId = (int)fieldValue;
                break;
            case 1:
                ProjectName = (string)fieldValue;
                break;
            default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
        }

        ;
    }
}