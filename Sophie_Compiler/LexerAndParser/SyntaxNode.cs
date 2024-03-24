namespace Sophie_Compiler.LexerAndParser;

public abstract class SyntaxNode
{
    public abstract SyntaxKind SyntaxKind { get; }
    public abstract IEnumerable<SyntaxNode> GetChildren();
}