namespace Compiler.CodeAnalysis.Binding;

internal class BoundLabelStatement : BoundStatement
{
    public LabelSymbol Label { get; }

    public BoundLabelStatement(LabelSymbol label)
    {
        Label = label;
    }

    public override BoundNodeKind Kind => BoundNodeKind.LabelStatement;
}