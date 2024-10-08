﻿namespace Compiler.CodeAnalysis.Binding;

public enum BoundBinaryOperatorKind
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
    LogicalAnd,
    LogicalOr,
    Equals,
    NotEquals,
    Less,
    LessOrEquals,
    Greater,
    GreaterOrEquals,
    BitwiseAnd,
    BitwiseOr,
    BitwiseXor
}