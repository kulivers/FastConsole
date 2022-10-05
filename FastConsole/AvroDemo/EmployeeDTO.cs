using System.Runtime.Serialization;
using Avro;
using Avro.Specific;
using SolTechnology.Avro;

namespace FastConsole;

public class EmployeeDTO : ISpecificRecord
{
    private static Schema _SCHEMA = Schema.Parse(File.ReadAllText(@"D:\Playground\Garbage\FastConsole\FastConsole\generatedSchema.json"));
    // private static Schema _SCHEMA2 = Schema.Parse(AvroConvert.GenerateSchema(typeof(EmployeeDTO)));
    public Schema Schema => _SCHEMA;

    [DataMember(Name = nameof(Name))]
    public string Name { get; set; }

    [DataMember(Name = nameof(EmployeeId))]
    public int EmployeeId { get; set; }
    [DataMember(Name = nameof(Reportees))]

    public IList<int> Reportees { get; set; }
    [DataMember(Name = nameof(Notes))]

    public string Notes { get; set; }

    [DataMember(Name = nameof(ActiveProjects))]
    public IList<ProjectDTO> ActiveProjects { get; set; }
    [DataMember(Name = nameof(RawBytes))]

    public byte[] RawBytes { get; set; }

    [DataMember(Name = nameof(StillWorksHere))]
    public bool StillWorksHere { get; set; }

    public object Get(int fieldPos)
    {
        switch (fieldPos)
        {
            case 0: return Name;
            case 1: return EmployeeId;
            case 2: return Reportees;
            case 3: return Notes;
            case 4: return ActiveProjects;
            case 5: return RawBytes;
            case 6: return StillWorksHere;
            default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
        }
    }

    public void Put(int fieldPos, object fieldValue)
    {
        switch (fieldPos)
        {
            case 0:
                Name = (string)fieldValue;
                break;
            case 1:
                EmployeeId = (int)fieldValue;
                break;
            case 2:
                Reportees = (IList<int>)fieldValue;
                break;
            case 3:
                Notes = (string)fieldValue;
                break;
            case 4:
                ActiveProjects = (IList<ProjectDTO>)fieldValue;
                break;
            case 5:
                RawBytes = (byte[])fieldValue;
                break;
            case 6:
                StillWorksHere = (bool)fieldValue;
                break;
            default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
        }

        ;
    }
}


public class EmployeeDTOStrWrapper : ISpecificRecord
{
    private EmployeeDTOStr _employeeDtoStr;

    public EmployeeDTOStrWrapper(EmployeeDTOStr employeeDtoStr)
    {
        _employeeDtoStr = employeeDtoStr;
    }
    public object Get(int fieldPos)
    {
        switch (fieldPos)
        {
            case 0: return _employeeDtoStr.Name;
            case 1: return _employeeDtoStr.EmployeeId;
            case 2: return _employeeDtoStr.Reportees;
            case 3: return _employeeDtoStr.Notes;
            case 4: return _employeeDtoStr.ActiveProjects;
            case 5: return _employeeDtoStr.RawBytes;
            case 6: return _employeeDtoStr.StillWorksHere;
            default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
        }
    }

    public void Put(int fieldPos, object fieldValue)
    {
        switch (fieldPos)
        {
            case 0:
                _employeeDtoStr.Name = (string)fieldValue;
                break;
            case 1:
                _employeeDtoStr.EmployeeId = (int)fieldValue;
                break;
            case 2:
                _employeeDtoStr.Reportees = (IList<int>)fieldValue;
                break;
            case 3:
                _employeeDtoStr.Notes = (string)fieldValue;
                break;
            case 4:
                _employeeDtoStr.ActiveProjects = (IList<ProjectDTO>)fieldValue;
                break;
            case 5:
                _employeeDtoStr.RawBytes = (byte[])fieldValue;
                break;
            case 6:
                _employeeDtoStr.StillWorksHere = (bool)fieldValue;
                break;
            default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
        }

        ;
    }

    public Schema Schema { get; }
}
public struct EmployeeDTOStr : ISpecificRecord
{
    public static Schema _SCHEMA = Schema.Parse(
        @"{""type"":""record"",""name"":""EmployeeDTO"",""namespace"":""FastConsole"",""fields"":[{""name"":""Name"",""type"":""string""},{""name"":""EmployeeId"",""type"":""int""},{""name"":""Reportees"",""type"":{""type"":""array"",""items"":""int""}},{""name"":""Notes"",""type"":[""null"",""string""]},{""name"":""ActiveProjects"",""type"":{""type"":""array"",""items"":{""type"":""record"",""name"":""ProjectDTO"",""namespace"":""FastConsole"",""fields"":[{""name"":""ProjectId"",""type"":""int""},{""name"":""ProjectName"",""type"":""string""}]}}},{""name"":""RawBytes"",""type"":""bytes""},{""name"":""StillWorksHere"",""type"":""boolean""}]}");

    public Schema Schema => _SCHEMA;

    public string Name { get; set; }

    public int EmployeeId { get; set; }

    public IList<int> Reportees { get; set; }

    public string Notes { get; set; }

    public IList<ProjectDTO> ActiveProjects { get; set; }

    public byte[] RawBytes { get; set; }

    public bool StillWorksHere { get; set; }

    public object Get(int fieldPos)
    {
        switch (fieldPos)
        {
            case 0: return Name;
            case 1: return EmployeeId;
            case 2: return Reportees;
            case 3: return Notes;
            case 4: return ActiveProjects;
            case 5: return RawBytes;
            case 6: return StillWorksHere;
            default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
        }
    }

    public void Put(int fieldPos, object fieldValue)
    {
        switch (fieldPos)
        {
            case 0:
                Name = (string)fieldValue;
                break;
            case 1:
                EmployeeId = (int)fieldValue;
                break;
            case 2:
                Reportees = (IList<int>)fieldValue;
                break;
            case 3:
                Notes = (string)fieldValue;
                break;
            case 4:
                ActiveProjects = (IList<ProjectDTO>)fieldValue;
                break;
            case 5:
                RawBytes = (byte[])fieldValue;
                break;
            case 6:
                StillWorksHere = (bool)fieldValue;
                break;
            default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
        }

        ;
    }
   
}