namespace Compiler.CodeAnalysis.Syntax;

public sealed class IfStatementSyntax : StatementSyntax
{
    public override SyntaxKind SyntaxKind => SyntaxKind.IfStatement;
    public SyntaxToken IfKeyword { get; }
    public ExpressionSyntax Condition { get; }
    public StatementSyntax ThanStatement { get; }
    public ElseClauseSyntax ElseClause { get; }

  

    public IfStatementSyntax(SyntaxToken ifKeyword, ExpressionSyntax condition,
                           StatementSyntax thanStatement, ElseClauseSyntax elseClause)
    {
        IfKeyword = ifKeyword;
        Condition = condition;
        ThanStatement = thanStatement;
        ElseClause = elseClause;
    }
}