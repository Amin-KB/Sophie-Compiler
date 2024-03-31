using Compiler.CodeAnalysis.Syntax;

namespace Compiler.CodeAnalysis.Binding;

internal sealed class BoundAssignmentExpression : BoundExpression
{
    public BoundAssignmentExpression(string name, BoundExpression boundExpression)
    {
        Name = name;
        Expression = boundExpression;
    }

    public BoundExpression Expression { get; }

    public string Name { get;  }

    public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;
    public override Type Type { get; }
}