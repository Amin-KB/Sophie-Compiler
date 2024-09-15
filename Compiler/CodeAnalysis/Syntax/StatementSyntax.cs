namespace Compiler.CodeAnalysis.Syntax;

public abstract class StatementSyntax:SyntaxNode
{
    public override SyntaxKind SyntaxKind { get; }
}