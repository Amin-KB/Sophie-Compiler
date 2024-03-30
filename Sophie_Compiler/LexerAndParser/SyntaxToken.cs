namespace Sophie_Compiler.LexerAndParser;

public sealed class SyntaxToken:SyntaxNode
{
    public override SyntaxKind SyntaxKind { get;  }
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        return Enumerable.Empty<SyntaxNode>();
    }

    public int Position { get;  }
    public string Text { get;  }
    public object Value { get;  }
    public SyntaxToken(SyntaxKind kind,int position,string text,object value)
    {
        SyntaxKind = kind;
        Position = position;
        Text = text;
        Value = value;
    }
}