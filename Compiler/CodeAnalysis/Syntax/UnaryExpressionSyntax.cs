namespace Compiler.CodeAnalysis.Syntax;

public class UnaryExpressionSyntax:ExpressionSyntax
{
    public override SyntaxKind SyntaxKind => SyntaxKind.UnaryExpression;


    public SyntaxToken OperatorToken { get; }

    public ExpressionSyntax Operand { get; }

 
  

  
    public UnaryExpressionSyntax(SyntaxToken operatorToken,ExpressionSyntax operand)
    {
      
        Operand=operand;
        OperatorToken = operatorToken;
    }
}