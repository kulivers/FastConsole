using YamlDotNet.Core;

public class MyParser : Parser
{
    public MyParser(TextReader input) : base(new Scanner(input, false))
    {
    }

    public MyParser(IScanner scanner) : base(scanner)
    {
        throw new NotImplementedException();
    }
}