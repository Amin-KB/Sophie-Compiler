﻿namespace Compiler.CodeAnalysis.Syntax;

public sealed class LiteralExpressionSyntax:ExpressionSyntax
{
    public override SyntaxKind SyntaxKind => SyntaxKind.LiteralExpression;
 

    public SyntaxToken LiteralToken { get;  }
    public object Value { get;  }

    public LiteralExpressionSyntax(SyntaxToken literalToken):this(literalToken,literalToken.Value)
    {
      
    }
    public LiteralExpressionSyntax(SyntaxToken literalToken,object value)
    {
        LiteralToken = literalToken;
        Value = value;
    }
}