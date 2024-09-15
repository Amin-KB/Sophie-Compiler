namespace Compiler.CodeAnalysis.Binding;

internal sealed class BoundIfStatement :BoundStatement
{
    public override BoundNodeKind Kind => BoundNodeKind.IfStatement;
    public BoundExpression Condition { get; }
    public BoundStatement ThanStatement { get; }
    public BoundStatement ElseStatement { get; }

    public BoundIfStatement(BoundExpression condition, BoundStatement thanStatement,
                          BoundStatement elseStatement)
    {
        Condition = condition;
        ThanStatement = thanStatement;
        ElseStatement = elseStatement;
    }

}