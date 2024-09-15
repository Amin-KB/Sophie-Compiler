namespace Compiler.CodeAnalysis.Syntax;

public sealed class ExpressionStatementSyntax:StatementSyntax
{
    public ExpressionSyntax Expression { get; }
    public override SyntaxKind SyntaxKind => SyntaxKind.ExpressionStatement;

    public ExpressionStatementSyntax(ExpressionSyntax expression)
    {
        Expression = expression;
    }
}