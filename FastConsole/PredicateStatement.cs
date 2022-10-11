using System.Runtime.Serialization;
using Avro;
using Avro.Specific;

[DataContract]
public class PredicateStatement : ISpecificRecord
{
    [DataMember] public Predicate Predicate;
    [DataMember] public Wrapped[] Facts;

    public PredicateStatement(Predicate predicate, IEnumerable<Wrapped> result)
    {
        Predicate = predicate;
        Facts = result.ToArray();
        Name = predicate.Name;
    }

    public PredicateStatement()
    {
    }


    [DataMember] public object Name { get; set; }

    public override int GetHashCode()
    {
        return Predicate.GetHashCode();
    }

    public object Get(int fieldPos)
    {
        switch (fieldPos)
        {
            case 0:
                return Predicate;
            case 1:
                return Facts;
            case 2:
                return Name;
            default:
                throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
        }
    }

    public void Put(int fieldPos, object fieldValue)
    {
        switch (fieldPos)
        {
            case 0:
                Predicate = (Predicate)fieldValue;
                break;
            case 1:
                Facts = (Wrapped[])fieldValue;
                break;
            case 2:
                Name = (string)fieldValue;
                break;
            default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
        }
    }

    public Schema Schema => Schema.Parse(File.ReadAllText(@"D:\Playground\Garbage\FastConsole\FastConsole\predicateStatementSchema.json"));
}

[DataContract]
public class Wrapped : IWrapped
{
    [DataMember] public string Name { get; set; }

    public Wrapped(string name)
    {
        Name = name;
    }

    public Wrapped()
    {
    }

    public object Get(int fieldPos)
    {
        return Name;
    }

    public void Put(int fieldPos, object fieldValue)
    {
        Name = (string)fieldValue;
    }

    public Schema Schema => Schema.Parse(File.ReadAllText(@"D:\Playground\Garbage\FastConsole\FastConsole\wrappedSchema.json"));
}

public interface IWrapped : ISpecificRecord
{
    public string Name { get; set; }
}

[DataContract]
public struct Predicate : ISpecificRecord
{
    [DataMember] public CanonicalName Name;
    public Predicate(string name)
    {
        Name = new CanonicalName(name, name);
        const string path = @"D:\Playground\Garbage\FastConsole\FastConsole\PredicateSchema.json";
        Schema = Schema.Parse(File.ReadAllText(path));
    }

    public object Get(int fieldPos)
    {
        return Name;
    }

    public void Put(int fieldPos, object fieldValue)
    {
        Name = (CanonicalName)fieldValue;
    }

    public Schema Schema { get; }
}

[DataContract]
public class CanonicalName : ISpecificRecord
{
    [DataMember] public string Name { get; set; }
    [DataMember] public string FullName { get; set; }

    public CanonicalName()
    {
        
    }

    public CanonicalName(string name, string fullName)
    {
        Name = name;
        FullName = fullName;
    }

    public object Get(int fieldPos)
    {
        switch (fieldPos)
        {
            case 0:
            {
                return Name;
            }
            case 1:
            {
                return FullName;
            }
            default:
                throw new Exception("No");
        }
    }

    public void Put(int fieldPos, object fieldValue)
    {
        switch (fieldPos)
        {
            case 0:
            {
                Name = (string)fieldValue;
                break;
            }
            case 1:
            {
                FullName = (string)fieldValue;
                break;
            }
            default:
                throw new Exception("No");
        }
    }

    public Schema Schema { get; }
}