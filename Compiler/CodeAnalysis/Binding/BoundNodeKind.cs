﻿namespace Compiler.CodeAnalysis.Binding;

public enum BoundNodeKind
{
    
    //Expressions
    LiteralExpression,
    UnaryExpression,
    VariableExpression,
    AssignmentExpression,
    BinaryExpression,
    
    //Statements
    BlockStatement,
    ExpressionStatement,
    IfStatement,
    WhileStatement,
    ForStatement,
    GotoStatement,
    LabelStatement,
    ConditionalGotoStatement,
    //Declarations
    VariableDeclaration,
    ErrorExpression,
    CallExpression,
    ConversionExpression
}