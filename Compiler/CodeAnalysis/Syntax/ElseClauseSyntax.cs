namespace Compiler.CodeAnalysis.Syntax;

public sealed class ElseClauseSyntax:SyntaxNode
{
    public override SyntaxKind SyntaxKind => SyntaxKind.ElseClause;
    public SyntaxToken ElseKeyword { get; }
    public StatementSyntax ElseStatement { get; }

    public ElseClauseSyntax(SyntaxToken elseKeyword, StatementSyntax elseStatement)
    {
        ElseKeyword = elseKeyword;
        ElseStatement = elseStatement;
    }
 
    

}