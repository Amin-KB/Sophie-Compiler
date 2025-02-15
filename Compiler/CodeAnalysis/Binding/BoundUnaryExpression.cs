using Compiler.CodeAnalysis.Symbols;

namespace Compiler.CodeAnalysis.Binding;

internal sealed class BoundUnaryExpression:BoundExpression
{
   
    public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
    public override TypeSymbol Type => Op.ResultType;
    public BoundUnaryOperator Op { get; }
    public BoundExpression Operand { get; }

    public BoundUnaryExpression(BoundUnaryOperator op,BoundExpression operand)
    {
        Op = op;
        Operand = operand;
    }
}