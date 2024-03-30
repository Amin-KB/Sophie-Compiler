namespace Compiler.CodeAnalysis.Syntax;

public abstract class SyntaxNode
{
    public abstract SyntaxKind SyntaxKind { get; }
    public abstract IEnumerable<SyntaxNode> GetChildren();
}