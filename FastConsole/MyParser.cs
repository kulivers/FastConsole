using YamlDotNet.Core;
using YamlDotNet.Core.Tokens;

public class MyScanner : Scanner
{
    public bool MoveNext()
    {
        var moveNext = base.MoveNext();
        return moveNext;
    }

    public bool MoveNextWithoutConsuming()
    {
        var moveNextWithoutConsuming = base.MoveNextWithoutConsuming();
        return moveNextWithoutConsuming;
    }

    public void ConsumeCurrent()
    {
        base.ConsumeCurrent();
    }

    public Mark CurrentPosition
    {
        get
        {
            var currentPosition = base.CurrentPosition;
            return currentPosition;
        }
    }

    public Token? Current
    {
        get
        {
            var current = base.Current;
            return current;
        }
    }

    public MyScanner(TextReader input, bool skipComments = false) : base(input, skipComments)
    {
    }
}

public class MyParser : Parser
{
    public MyParser(TextReader input) : base(new MyScanner(input, false))
    {
    }

    public MyParser(IScanner scanner) : base(scanner)
    {
        
    }
}