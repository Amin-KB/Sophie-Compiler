namespace Compiler.CodeAnalysis.Syntax;

public sealed class CompilationUnitSyntax :SyntaxNode
{
    

    public CompilationUnitSyntax(ExpressionSyntax expression,SyntaxToken endOfFileToken)
    {
        Expression = expression;
        EndOfFileToken = endOfFileToken;
    }

    public override SyntaxKind SyntaxKind => SyntaxKind.CompilationUnit;
    public ExpressionSyntax Expression { get; }
    public SyntaxToken EndOfFileToken { get; }
}