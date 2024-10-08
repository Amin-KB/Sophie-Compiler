using System.Collections.Immutable;

namespace Compiler.CodeAnalysis.Binding;

internal sealed class BoundBlockStatement:BoundStatement
{
    public ImmutableArray<BoundStatement> Statements { get; }
    public override BoundNodeKind Kind => BoundNodeKind.BlockStatement;

    public BoundBlockStatement(ImmutableArray<BoundStatement> statements)
    {
        Statements = statements;
    }
}