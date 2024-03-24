namespace Sophie_Compiler.LexerAndParser;

public enum SyntaxKind
{
    NumberToken,
    WhiteSpaceToken,
    PlusToken,
    MinusToken,
    StarToken,
    SlashToken,
    OpenParanthesisToken,
    CloseParanthesisToken,
    BadToken,
    EndOfFileToken,
    NumberExpression,
    BinaryExpression,
    ParenthesizedExpression
}