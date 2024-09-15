namespace Compiler.CodeAnalysis.Syntax;

public sealed class CompilationUnitSyntax :SyntaxNode
{
    public CompilationUnitSyntax(StatementSyntax statement,SyntaxToken endOfFileToken)
    {
        Statement = statement;
        EndOfFileToken = endOfFileToken;
    }

    public override SyntaxKind SyntaxKind => SyntaxKind.CompilationUnit;
    public StatementSyntax Statement { get; }
    public SyntaxToken EndOfFileToken { get; }
}