using Compiler.CodeAnalysis.Symbols;

namespace Compiler.CodeAnalysis.Binding;

internal sealed class BoundBinaryExpression:BoundExpression
{
    public override TypeSymbol Type => Op.ResultType;
    public override BoundNodeKind Kind => BoundNodeKind.BinaryExpression;
    public BoundBinaryOperator Op { get; }
    public BoundExpression Right { get; }
    public BoundExpression Left { get; }

    public BoundBinaryExpression(BoundExpression left,BoundBinaryOperator op,BoundExpression right)
    {
        Op = op;
        Right = right;
        Left = left;
    }
}