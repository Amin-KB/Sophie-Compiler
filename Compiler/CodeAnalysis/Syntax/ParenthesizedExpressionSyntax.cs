namespace Compiler.CodeAnalysis.Syntax;

public sealed class ParenthesizedExpressionSyntax:ExpressionSyntax
{
    public override SyntaxKind SyntaxKind => SyntaxKind.ParenthesizedExpression;
    public SyntaxToken OpenParenthesisToken { get; }
    public ExpressionSyntax Expression { get; }
    public SyntaxToken CloseParenthesisToken { get;  }

    public ParenthesizedExpressionSyntax(SyntaxToken openParenthesisToken,ExpressionSyntax expression
        ,SyntaxToken closeParenthesisToken)
    {
        OpenParenthesisToken = openParenthesisToken;
        Expression = expression;
        CloseParenthesisToken = closeParenthesisToken;
    }

   

}