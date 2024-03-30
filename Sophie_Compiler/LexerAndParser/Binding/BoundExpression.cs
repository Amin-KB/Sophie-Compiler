namespace Sophie_Compiler.LexerAndParser.Binding;

public abstract class BoundExpression:BoundNode
{
  
    public abstract Type Type { get; }
}