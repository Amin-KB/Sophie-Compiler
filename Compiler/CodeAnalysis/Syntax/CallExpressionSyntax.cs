﻿using System.Collections.Immutable;

namespace Compiler.CodeAnalysis.Syntax;
public class CallExpressionSyntax : ExpressionSyntax
{

    public CallExpressionSyntax(SyntaxToken identifier,SyntaxToken openParenthesisToken, SeparatedSyntaxList<ExpressionSyntax> arguments, SyntaxToken closeParenthesisToken)
    {
        Identifier = identifier;
        OpenParenthesisToken = openParenthesisToken;
        Arguments = arguments;
        CloseParenthesisToken = closeParenthesisToken;
    }
    public override SyntaxKind SyntaxKind => SyntaxKind.CallExpression;

    public SyntaxToken Identifier { get; }
    public SyntaxToken OpenParenthesisToken { get; }
    public SeparatedSyntaxList<ExpressionSyntax> Arguments { get; }
    public SyntaxToken CloseParenthesisToken { get; }
}
