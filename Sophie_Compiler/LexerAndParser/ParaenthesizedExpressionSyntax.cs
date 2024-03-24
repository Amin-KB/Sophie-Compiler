namespace Sophie_Compiler.LexerAndParser;

public sealed class ParaenthesizedExpressionSyntax:ExpressionSyntax
{
    public SyntaxToken OpenParenthesisToken { get; set; }
    public SyntaxToken CloseParenthesisToken { get; set; }
    public ExpressionSyntax Expression { get; set; }
    public ParaenthesizedExpressionSyntax(SyntaxToken openParenthesisToken,ExpressionSyntax expression
        ,SyntaxToken closeParenthesisToken)
    {
        OpenParenthesisToken = openParenthesisToken;
        Expression = expression;
        CloseParenthesisToken = closeParenthesisToken;
    }

    public override SyntaxKind SyntaxKind => SyntaxKind.ParenthesizedExpression;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return OpenParenthesisToken;
        yield return Expression;
        yield return CloseParenthesisToken;
    }
}