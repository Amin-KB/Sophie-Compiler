namespace Sophie_Compiler.LexerAndParser;

public enum SyntaxKind
{
    //Tokens
    NumberToken,
    WhiteSpaceToken,
    PlusToken,
    MinusToken,
    StarToken,
    SlashToken,
    OpenParenthesisToken,
    CloseParenthesisToken,
    BadToken,
    EndOfFileToken,
  
    //Expressions
    LiteralExpression,
    BinaryExpression,
    ParenthesizedExpression,
    UnaryExpression,
    TrueKeyword,
    FalseKeyword,
    IdentifierToken,
    BangToken,
    AmpersandAmperSandToken,
    PipePipeToken
}