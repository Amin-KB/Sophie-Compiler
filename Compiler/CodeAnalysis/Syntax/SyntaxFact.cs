namespace Compiler.CodeAnalysis.Syntax;

public static class SyntaxFact
{
    public static int GetUnaryOperatorPrecedence(this SyntaxKind kind)
    {
        switch (kind)
        {
           
            case SyntaxKind.PlusToken:
            case SyntaxKind.MinusToken:
            case SyntaxKind.BangToken:
                return 6;
            default:
                return 0;
          
        }
    }
    public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
    {
        switch (kind)
        {
            case SyntaxKind.StarToken:
            case SyntaxKind.SlashToken:
                return 5;
            case SyntaxKind.PlusToken:
            case SyntaxKind.MinusToken:
                return 4;
            case SyntaxKind.EqualEqualToken:
            case SyntaxKind.BangEqualToken:
                return 3;
            case SyntaxKind.AmpersandAmperSandToken:
                return 2;
            case SyntaxKind.PipePipeToken:
                return 1;
            default:
                return 0;
          
        }
    }

    public static SyntaxKind GetKeywordKind(string text)
    {
        switch (text)
        {
            case "true":
                return SyntaxKind.TrueKeyword;
            case "false":
                return SyntaxKind.FalseKeyword;
            default:
                return SyntaxKind.IdentifierToken;
        }
    }

    public static string GetText(SyntaxKind kind)
    {
        switch (kind)
        {
            case SyntaxKind.PlusToken:
                return "+";
            case SyntaxKind.MinusToken:
                return "-";
            case SyntaxKind.StarToken:
                return "*";
            case SyntaxKind.SlashToken:
                return "/";
            case SyntaxKind.OpenParenthesisToken:
                return "(";
            case SyntaxKind.CloseParenthesisToken:
                return ")";
            case SyntaxKind.TrueKeyword:
                return "true";
            case SyntaxKind.FalseKeyword:
                return "false";
            case SyntaxKind.BangToken:
                return "!";
            case SyntaxKind.AmpersandAmperSandToken:
                return "&&";
            case SyntaxKind.PipePipeToken:
                return "||";
            case SyntaxKind.EqualEqualToken:
                return "==";
            case SyntaxKind.BangEqualToken:
                return "!=";
            case SyntaxKind.EqualToken:
                return "=";
            default:
                return null;
            
        }
    }
}