namespace Compiler.CodeAnalysis.Binding;

internal sealed class BoundWhileStatement: BoundStatement
{
    public override BoundNodeKind Kind => BoundNodeKind.WhileStatement;
    public BoundExpression Condition { get; }
    public BoundStatement Body { get; }

    public BoundWhileStatement(BoundExpression condition, BoundStatement body)
    {
        Condition = condition;
        Body = body;
    }

   
}