namespace Compiler.CodeAnalysis;

public class Diagnostic
{
    public Diagnostic(TextSpan textSpan,string message)
    {
        TextSpan = textSpan;
        Message = message;
    }

    public string Message { get;  }
    public TextSpan TextSpan { get;  }
    public override string ToString() => Message;

}