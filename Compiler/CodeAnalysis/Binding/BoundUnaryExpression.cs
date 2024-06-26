﻿namespace Compiler.CodeAnalysis.Binding;

internal sealed class BoundUnaryExpression:BoundExpression
{
    public override Type Type => Op.ResultType;
    public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
    public BoundUnaryOperator Op { get; }
    public BoundExpression Operand { get; }

    public BoundUnaryExpression(BoundUnaryOperator op,BoundExpression operand)
    {
        Op = op;
        Operand = operand;
    }
}