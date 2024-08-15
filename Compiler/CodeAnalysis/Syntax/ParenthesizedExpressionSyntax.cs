namespace Compiler.CodeAnalysis.Syntax;

public sealed class ParenthesizedExpressionSyntax:ExpressionSyntax
{
    public SyntaxToken OpenParenthesisToken { get; set; }
    public SyntaxToken CloseParenthesisToken { get; set; }
    public ExpressionSyntax Expression { get; set; }
    public ParenthesizedExpressionSyntax(SyntaxToken openParenthesisToken,ExpressionSyntax expression
        ,SyntaxToken closeParenthesisToken)
    {
        OpenParenthesisToken = openParenthesisToken;
        Expression = expression;
        CloseParenthesisToken = closeParenthesisToken;
    }

    public override SyntaxKind SyntaxKind => SyntaxKind.ParenthesizedExpression;

}