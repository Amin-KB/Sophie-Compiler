namespace Sophie_Compiler.LexerAndParser.Binding;

public class BoundBinaryExpression:BoundExpression
{
    public override Type Type => Right.Type;
    public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
    public BoundBinaryOperatorKind OperatorKind { get; }
    public BoundExpression Right { get; }
    public BoundExpression Left { get; }

    public BoundBinaryExpression(BoundExpression left,BoundBinaryOperatorKind operatorKind,BoundExpression right)
    {
        OperatorKind = operatorKind;
        Right = right;
        Left = left;
    }
}