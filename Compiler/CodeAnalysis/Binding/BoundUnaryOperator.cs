using Compiler.CodeAnalysis.Symbols;
using Compiler.CodeAnalysis.Syntax;

namespace Compiler.CodeAnalysis.Binding;

internal sealed class BoundUnaryOperator
{
    private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, TypeSymbol operandType, TypeSymbol resultType)
    {
        SyntaxKind = syntaxKind;
        Kind = kind;
        OperandType = operandType;
        ResultType = resultType;
    }
    private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, TypeSymbol operandType):this(syntaxKind,kind,operandType,operandType)
    {
    }

    private static BoundUnaryOperator[] _operators =
    {
        new BoundUnaryOperator(SyntaxKind.BangToken, BoundUnaryOperatorKind.LogicalNegation, TypeSymbol.Bool),
        new BoundUnaryOperator(SyntaxKind.PlusToken, BoundUnaryOperatorKind.Identity, TypeSymbol.Int),
        new BoundUnaryOperator(SyntaxKind.MinusToken, BoundUnaryOperatorKind.Negation, TypeSymbol.Int),
        new BoundUnaryOperator(SyntaxKind.TildeToken, BoundUnaryOperatorKind.OnesComplement, TypeSymbol.Int),
    };

    public static BoundUnaryOperator Bind(SyntaxKind syntaxKind, TypeSymbol type)
    {
        foreach (var op in _operators)
        {
            if (op.SyntaxKind == syntaxKind && op.OperandType == type)
                return op;
        }

        return null;
    }
    public SyntaxKind SyntaxKind { get;  }
    public TypeSymbol ResultType { get;  }
    public BoundUnaryOperatorKind Kind { get; }
    public TypeSymbol OperandType { get;  }

  
}