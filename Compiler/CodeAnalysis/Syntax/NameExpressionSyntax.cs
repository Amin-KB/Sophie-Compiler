namespace Compiler.CodeAnalysis.Syntax;

public sealed class NameExpressionSyntax : ExpressionSyntax
{
    public NameExpressionSyntax(SyntaxToken identifierToken)
    {
        IdentifierToken = identifierToken;
    }

    public override SyntaxKind SyntaxKind => SyntaxKind.NameExpression;
    public SyntaxToken IdentifierToken { get; }

   


}