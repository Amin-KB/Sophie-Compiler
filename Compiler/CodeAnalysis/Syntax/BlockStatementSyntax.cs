using System.Collections.Immutable;

namespace Compiler.CodeAnalysis.Syntax;

public sealed class BlockStatementSyntax:StatementSyntax
{
    public override SyntaxKind SyntaxKind => SyntaxKind.BlockStatement;
    public SyntaxToken OpenBraceToken { get; }
    public ImmutableArray<StatementSyntax> Statements { get; }
    public SyntaxToken CloseBraceToken { get; }

    public BlockStatementSyntax(SyntaxToken openBraceToken,ImmutableArray<StatementSyntax> statements,
                                SyntaxToken closeBraceToken)
    {
        OpenBraceToken = openBraceToken;
        Statements = statements;
        CloseBraceToken = closeBraceToken;
    }
}