using Compiler.CodeAnalysis.Syntax;

namespace Compiler.CodeAnalysis.Binding;

internal sealed class BoundAssignmentExpression : BoundExpression
{
    public BoundAssignmentExpression(VariableSymbol variable, BoundExpression boundExpression)
    {
        Variable = variable;
        Expression = boundExpression;
    }

    public BoundExpression Expression { get; }

    public VariableSymbol Variable { get;  }

    public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;
    public override Type Type => Variable.Type;
}