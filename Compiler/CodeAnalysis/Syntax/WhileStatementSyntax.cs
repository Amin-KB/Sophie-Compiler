namespace Compiler.CodeAnalysis.Syntax;

public class WhileStatementSyntax:StatementSyntax
{
    public override SyntaxKind SyntaxKind => SyntaxKind.WhileStatement;
    public SyntaxToken WhileToken { get; }
    public ExpressionSyntax Condition { get; }
    public StatementSyntax Body { get; }

    public WhileStatementSyntax(SyntaxToken whileToken,ExpressionSyntax condition,StatementSyntax body)
    {
        WhileToken = whileToken;
        Condition = condition;
        Body = body;
    }
}