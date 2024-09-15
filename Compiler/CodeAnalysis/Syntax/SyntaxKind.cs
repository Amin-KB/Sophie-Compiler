namespace Compiler.CodeAnalysis.Syntax;

public enum SyntaxKind
{
    //Tokens
    EndOfFileToken,
    NumberToken,
    WhiteSpaceToken,
    PlusToken,
    MinusToken,
    StarToken,
    SlashToken,
    OpenParenthesisToken,
    CloseParenthesisToken,
    BangToken,
    EqualToken,
    BadToken,
    AmpersandAmpersandToken,
    PipePipeToken,
    EqualEqualToken,
    IdentifierToken,
    BangEqualToken,
    OpenBraceToken,
    CloseBraceToken,
    LessOrEqualToken,
    LessToken,
    GreaterOrEqualToken,
    GreaterToken,
    //Expressions
    LiteralExpression,
    BinaryExpression,
    ParenthesizedExpression,
    UnaryExpression,
    NameExpression,
    AssignmentExpression,
    
    //Keywords
    TrueKeyword,
    FalseKeyword,
    LetKeyword,
    VarKeyword,
    
    
    //Nodes
    CompilationUnit,
    
    //Statements
    BlockStatement,
    ExpressionStatement,

    //Declarations
    VariableDeclaration,


    
}