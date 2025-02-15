namespace Compiler.CodeAnalysis.Binding;

internal class BoundLabelStatement : BoundStatement
{
    public BoundLabel Label { get; }

    public BoundLabelStatement(BoundLabel label)
    {
        Label = label;
    }

    public override BoundNodeKind Kind => BoundNodeKind.LabelStatement;
}