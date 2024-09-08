namespace Compiler.CodeAnalysis.Syntax;

public class AssignmentExpressionSyntax : ExpressionSyntax
{
 
    public override SyntaxKind SyntaxKind => SyntaxKind.AssignmentExpression;
    public SyntaxToken IdentifierToken { get; }
    public SyntaxToken EqualToken { get; }
    public ExpressionSyntax Expression { get; }

    public AssignmentExpressionSyntax(SyntaxToken identifierToken, SyntaxToken equalToken, ExpressionSyntax expression)
    {
        IdentifierToken = identifierToken;
        EqualToken = equalToken;
        Expression = expression;
    }


}