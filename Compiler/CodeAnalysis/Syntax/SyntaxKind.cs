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
  
  
    //Expressions
    LiteralExpression,
    BinaryExpression,
    ParenthesizedExpression,
    UnaryExpression,
    TrueKeyword,
    FalseKeyword,
    IdentifierToken,
  
    AmpersandAmperSandToken,
    PipePipeToken,
    EqualEqualToken,
   
    BangEqualToken,
    NameExpression,
    AssignmentEsxpression,
   
}