namespace Sophie_Compiler.LexerAndParser;

public class BinaryExpressionSyntax:ExpressionSyntax
{
    public override SyntaxKind SyntaxKind => SyntaxKind.BinaryExpression;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Left;
        yield return OperatorToken;
        yield return Right;
        
    }   

    public ExpressionSyntax Left { get; }
    public ExpressionSyntax Right { get; }
    public SyntaxToken OperatorToken { get; }
    public BinaryExpressionSyntax(ExpressionSyntax left,SyntaxToken operatorToken,ExpressionSyntax right)
    {
        Left = left;
        Right=right;
        OperatorToken = operatorToken;
    }
}