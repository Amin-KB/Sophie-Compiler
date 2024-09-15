using Compiler.CodeAnalysis.Syntax;

namespace Compiler.CodeAnalysis.Binding;

internal sealed class BoundBinaryOperator
{
    private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type leftType, Type rightType, Type resultType)
    {
        SyntaxKind = syntaxKind;
        Kind = kind;
        LeftType = leftType;
        RightType = rightType;
        ResultType = resultType;
        
    }

    private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type type)
        :this(syntaxKind,kind,type,type,type)
    {
      
        
    }

    private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type operandType,Type resultType)
        :this(syntaxKind,kind,operandType,operandType,resultType)
    {
      
        
    }
    private static BoundBinaryOperator[] _operators =
    {
        new BoundBinaryOperator(SyntaxKind.PlusToken, BoundBinaryOperatorKind.Addition, typeof(int)),
        new BoundBinaryOperator(SyntaxKind.MinusToken, BoundBinaryOperatorKind.Subtraction, typeof(int)),
        new BoundBinaryOperator(SyntaxKind.StarToken, BoundBinaryOperatorKind.Multiplication, typeof(int)),
        new BoundBinaryOperator(SyntaxKind.SlashToken, BoundBinaryOperatorKind.Division, typeof(int)),
        
        new BoundBinaryOperator(SyntaxKind.EqualEqualToken, BoundBinaryOperatorKind.Equals, typeof(int),typeof(bool)),
        new BoundBinaryOperator(SyntaxKind.BangEqualToken, BoundBinaryOperatorKind.NotEquals, typeof(int),typeof(bool)),
        new BoundBinaryOperator(SyntaxKind.LessToken, BoundBinaryOperatorKind.Less, typeof(int),typeof(bool)),
        new BoundBinaryOperator(SyntaxKind.LessOrEqualToken, BoundBinaryOperatorKind.LessOrEquals, typeof(int),typeof(bool)),
        new BoundBinaryOperator(SyntaxKind.GreaterToken, BoundBinaryOperatorKind.Greater, typeof(int),typeof(bool)),
        new BoundBinaryOperator(SyntaxKind.GreaterOrEqualToken, BoundBinaryOperatorKind.GreaterOrEquals, typeof(int),typeof(bool)),
        
        new BoundBinaryOperator(SyntaxKind.AmpersandAmpersandToken, BoundBinaryOperatorKind.LogicalAnd, typeof(bool)),
        new BoundBinaryOperator(SyntaxKind.PipePipeToken, BoundBinaryOperatorKind.LogicalOr, typeof(bool)),
        new BoundBinaryOperator(SyntaxKind.EqualEqualToken, BoundBinaryOperatorKind.Equals, typeof(bool)),
        new BoundBinaryOperator(SyntaxKind.BangEqualToken, BoundBinaryOperatorKind.NotEquals, typeof(bool)),
    };

    public static BoundBinaryOperator Bind(SyntaxKind syntaxKind, Type leftType,Type rightType)
    {
        foreach (var op in _operators)
        {
            if (op.SyntaxKind == syntaxKind && op.LeftType == leftType &&op.RightType==rightType)
                return op;
        }

        return null;
    }
    public SyntaxKind SyntaxKind { get;  }
    public Type ResultType { get;  }
    public Type RightType { get;  }

    public Type LeftType { get;  }
   

    public BoundBinaryOperatorKind Kind { get;  }
}