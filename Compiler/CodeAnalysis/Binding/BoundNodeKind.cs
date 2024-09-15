namespace Compiler.CodeAnalysis.Binding;

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
    
    //Declarations
    VariableDeclaration
}