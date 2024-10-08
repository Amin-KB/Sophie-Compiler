﻿using Compiler.CodeAnalysis.Syntax;

namespace Compiler.CodeAnalysis.Binding;

internal sealed class BoundUnaryOperator
{
    private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType, Type resultType)
    {
        SyntaxKind = syntaxKind;
        Kind = kind;
        OperandType = operandType;
        ResultType = resultType;
    }
    private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType):this(syntaxKind,kind,operandType,operandType)
    {
    }

    private static BoundUnaryOperator[] _operators =
    {
        new BoundUnaryOperator(SyntaxKind.BangToken, BoundUnaryOperatorKind.LogicalNegation, typeof(bool)),
        new BoundUnaryOperator(SyntaxKind.PlusToken, BoundUnaryOperatorKind.Identity, typeof(int)),
        new BoundUnaryOperator(SyntaxKind.MinusToken, BoundUnaryOperatorKind.Negation, typeof(int)),
        new BoundUnaryOperator(SyntaxKind.TildeToken, BoundUnaryOperatorKind.OnesComplement, typeof(int)),
    };

    public static BoundUnaryOperator Bind(SyntaxKind syntaxKind, Type type)
    {
        foreach (var op in _operators)
        {
            if (op.SyntaxKind == syntaxKind && op.OperandType == type)
                return op;
        }

        return null;
    }
    public SyntaxKind SyntaxKind { get;  }
    public Type ResultType { get;  }
    public BoundUnaryOperatorKind Kind { get; }
    public Type OperandType { get;  }

  
}