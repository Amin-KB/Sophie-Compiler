namespace Compiler.CodeAnalysis.Syntax;

public class UnaryExpressionSyntax:ExpressionSyntax
{
    public override SyntaxKind SyntaxKind => SyntaxKind.UnaryExpression;


    
 
    public ExpressionSyntax Operand { get; }

 
    public SyntaxToken OperatorToken { get; }

  
    public UnaryExpressionSyntax(SyntaxToken operatorToken,ExpressionSyntax operand)
    {
      
        Operand=operand;
        OperatorToken = operatorToken;
    }
}