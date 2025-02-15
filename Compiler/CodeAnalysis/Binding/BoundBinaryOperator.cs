using Compiler.CodeAnalysis.Symbols;
using Compiler.CodeAnalysis.Syntax;

namespace Compiler.CodeAnalysis.Binding;

internal sealed class BoundBinaryOperator
{
    private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, TypeSymbol leftType, TypeSymbol rightType, TypeSymbol resultType)
    {
        SyntaxKind = syntaxKind;
        Kind = kind;
        LeftType = leftType;
        RightType = rightType;
        ResultType = resultType;
        
    }

    private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, TypeSymbol type)
        :this(syntaxKind,kind,type,type,type)
    {
      
        
    }

    private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, TypeSymbol operandType, TypeSymbol resultType)
        :this(syntaxKind,kind,operandType,operandType,resultType)
    {
      
        
    }
    private static BoundBinaryOperator[] _operators =
    {
        new BoundBinaryOperator(SyntaxKind.PlusToken, BoundBinaryOperatorKind.Addition, TypeSymbol.Int),
        new BoundBinaryOperator(SyntaxKind.MinusToken, BoundBinaryOperatorKind.Subtraction, TypeSymbol.Int),
        new BoundBinaryOperator(SyntaxKind.StarToken, BoundBinaryOperatorKind.Multiplication, TypeSymbol.Int),
        new BoundBinaryOperator(SyntaxKind.SlashToken, BoundBinaryOperatorKind.Division, TypeSymbol.Int),
        
        new BoundBinaryOperator(SyntaxKind.AmpersandToken, BoundBinaryOperatorKind.BitwiseAnd,TypeSymbol.Int),
        new BoundBinaryOperator(SyntaxKind.PipeToken, BoundBinaryOperatorKind.BitwiseOr, TypeSymbol.Int),
        new BoundBinaryOperator(SyntaxKind.HatToken, BoundBinaryOperatorKind.BitwiseXor, TypeSymbol.Int),
        
        new BoundBinaryOperator(SyntaxKind.EqualEqualToken, BoundBinaryOperatorKind.Equals, TypeSymbol.Int,TypeSymbol.Bool),
        new BoundBinaryOperator(SyntaxKind.BangEqualToken, BoundBinaryOperatorKind.NotEquals, TypeSymbol.Int,TypeSymbol.Bool),
        new BoundBinaryOperator(SyntaxKind.LessToken, BoundBinaryOperatorKind.Less, TypeSymbol.Int,TypeSymbol.Bool),
        new BoundBinaryOperator(SyntaxKind.LessOrEqualToken, BoundBinaryOperatorKind.LessOrEquals, TypeSymbol.Int,TypeSymbol.Bool),
        new BoundBinaryOperator(SyntaxKind.GreaterToken, BoundBinaryOperatorKind.Greater, TypeSymbol.Int,TypeSymbol.Bool),
        new BoundBinaryOperator(SyntaxKind.GreaterOrEqualToken, BoundBinaryOperatorKind.GreaterOrEquals, TypeSymbol.Int,TypeSymbol.Bool),
        
        new BoundBinaryOperator(SyntaxKind.AmpersandToken, BoundBinaryOperatorKind.BitwiseAnd, TypeSymbol.Bool),
        new BoundBinaryOperator(SyntaxKind.AmpersandAmpersandToken, BoundBinaryOperatorKind.LogicalAnd, TypeSymbol.Bool),
        new BoundBinaryOperator(SyntaxKind.PipeToken, BoundBinaryOperatorKind.BitwiseOr,TypeSymbol.Bool),
        new BoundBinaryOperator(SyntaxKind.PipePipeToken, BoundBinaryOperatorKind.LogicalOr, TypeSymbol.Bool),
        new BoundBinaryOperator(SyntaxKind.HatToken, BoundBinaryOperatorKind.BitwiseXor, TypeSymbol.Bool),
        new BoundBinaryOperator(SyntaxKind.EqualEqualToken, BoundBinaryOperatorKind.Equals, TypeSymbol.Bool),
        new BoundBinaryOperator(SyntaxKind.BangEqualToken, BoundBinaryOperatorKind.NotEquals, TypeSymbol.Bool),

        new BoundBinaryOperator(SyntaxKind.PlusToken, BoundBinaryOperatorKind.Addition, TypeSymbol.String),
    };

    public static BoundBinaryOperator Bind(SyntaxKind syntaxKind, TypeSymbol leftType, TypeSymbol rightType)
    {
        foreach (var op in _operators)
        {
            if (op.SyntaxKind == syntaxKind && op.LeftType == leftType &&op.RightType==rightType)
                return op;
        }

        return null;
    }
    public SyntaxKind SyntaxKind { get;  }
    public TypeSymbol ResultType { get;  }
    public TypeSymbol RightType { get;  }

    public TypeSymbol LeftType { get;  }
   

    public BoundBinaryOperatorKind Kind { get;  }
}