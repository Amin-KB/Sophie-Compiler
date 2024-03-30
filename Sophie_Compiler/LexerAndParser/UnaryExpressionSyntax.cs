namespace Sophie_Compiler.LexerAndParser;

public class UnaryExpressionSyntax:ExpressionSyntax
{
    public override SyntaxKind SyntaxKind => SyntaxKind.UnaryExpression;

    public override IEnumerable<SyntaxNode> GetChildren()
    {
  
        yield return OperatorToken;
        yield return Operand;
        
    }
    
 
    public ExpressionSyntax Operand { get; }

 
    public SyntaxToken OperatorToken { get; }

  
    public UnaryExpressionSyntax(SyntaxToken operatorToken,ExpressionSyntax operand)
    {
      
        Operand=operand;
        OperatorToken = operatorToken;
    }
}