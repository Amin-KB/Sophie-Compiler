namespace Compiler.CodeAnalysis.Syntax;

public class AssignmentExpressionSyntax : ExpressionSyntax
{
    public AssignmentExpressionSyntax(SyntaxToken identifierToken, SyntaxToken equalToken, ExpressionSyntax expression)
    {
        IdentifierToken = identifierToken;
        EqualToken = equalToken;
        Expression = expression;
    }

    public ExpressionSyntax Expression { get; }

    public SyntaxToken EqualToken { get; }


    public SyntaxToken IdentifierToken { get; }

    public override SyntaxKind SyntaxKind => SyntaxKind.AssignmentEsxpression;


}