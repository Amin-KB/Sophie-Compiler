﻿namespace Compiler.CodeAnalysis.Syntax;

public sealed class SyntaxToken:SyntaxNode
{
    public override SyntaxKind SyntaxKind { get;  }


    public int Position { get;  }
    public string Text { get;  }
    public object Value { get;  }
    public TextSpan Span => new TextSpan(Position, Text.Length);
    public SyntaxToken(SyntaxKind kind,int position,string text,object value)
    {
        SyntaxKind = kind;
        Position = position;
        Text = text;
        Value = value;
    }
}