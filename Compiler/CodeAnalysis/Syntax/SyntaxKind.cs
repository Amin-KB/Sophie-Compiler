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
    AmpersandToken,
    PipePipeToken,
    PipeToken,
    EqualEqualToken,
    IdentifierToken,
    BangEqualToken,
    OpenBraceToken,
    CloseBraceToken,
    LessOrEqualToken,
    LessToken,
    GreaterOrEqualToken,
    GreaterToken,
    TildeToken,
    HatToken,
    StringToken,
    CommaToken,
    ColonToken,
    //Expressions
    LiteralExpression,
    BinaryExpression,
    ParenthesizedExpression,
    UnaryExpression,
    NameExpression,
    AssignmentExpression,
    CallExpression,
    //Keywords
    TrueKeyword,
    FalseKeyword,
    LetKeyword,
    VarKeyword,
    IfKeyword,
    ElseKeyword,
    WhileKeyword,
    ForKeyword,
    ToKeyword,
    
    //Nodes
    CompilationUnit,
    ElseClause,
    
    
    //Statements
    BlockStatement,
    ExpressionStatement,
    IfStatement,
    WhileStatement,
    ForStatement,
    //Declarations
    VariableDeclaration,


    TypeClause
}