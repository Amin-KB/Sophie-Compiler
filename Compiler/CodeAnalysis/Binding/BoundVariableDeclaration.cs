namespace Compiler.CodeAnalysis.Binding;

internal sealed class BoundVariableDeclaration:BoundStatement
{
    public override BoundNodeKind Kind => BoundNodeKind.VariableDeclaration;
    public VariableSymbol Variable { get; }
    public BoundExpression Initializer { get; }
 
    public BoundVariableDeclaration(VariableSymbol variable, BoundExpression initializer)
    {
        Variable = variable;
        Initializer = initializer;
    }
  
}