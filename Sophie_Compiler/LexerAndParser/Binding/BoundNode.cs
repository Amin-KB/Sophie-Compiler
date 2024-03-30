namespace Sophie_Compiler.LexerAndParser.Binding;

public abstract class BoundNode
{
    public abstract BoundNodeKind Kind { get; }
}