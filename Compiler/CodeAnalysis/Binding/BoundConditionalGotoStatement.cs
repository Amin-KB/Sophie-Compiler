namespace Compiler.CodeAnalysis.Binding;

internal sealed class BoundConditionalGotoStatement: BoundStatement
{
    public LabelSymbol Label { get; }
    public BoundExpression Condition { get; }
    public bool JumpIfTrue { get; }

    public BoundConditionalGotoStatement(LabelSymbol label, BoundExpression condition, bool jumpIfTrue=false)
    {
        Label = label;
        Condition = condition;
        JumpIfTrue = jumpIfTrue;
    }
    public override BoundNodeKind Kind => BoundNodeKind.ConditionalGotoStatement;
}