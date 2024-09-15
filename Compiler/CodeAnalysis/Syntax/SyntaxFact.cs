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
            case SyntaxKind.LessToken:
            case SyntaxKind.LessOrEqualToken:
            case SyntaxKind.GreaterToken:
            case SyntaxKind.GreaterOrEqualToken:
                return 3;
            case SyntaxKind.AmpersandAmpersandToken:
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
            case "else":
                return SyntaxKind.ElseKeyword;
            case "false":
                return SyntaxKind.FalseKeyword;
            case "for":
                return SyntaxKind.ForKeyword;
            case "if":
                return SyntaxKind.IfKeyword;
            case "let":
                return SyntaxKind.LetKeyword;
            case "to":
                return SyntaxKind.ToKeyword;
            case "true":
                return SyntaxKind.TrueKeyword;
            case "var":
                return SyntaxKind.VarKeyword;
            case "while":
                return SyntaxKind.WhileKeyword;
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
            case SyntaxKind.OpenBraceToken:
                return "{";
            case SyntaxKind.CloseBraceToken:
                return "}";
            case SyntaxKind.IfKeyword:
                return "if";
            case SyntaxKind.WhileKeyword:
                return "while";
            case SyntaxKind.ForKeyword:
                return "for";
            case SyntaxKind.ToKeyword:
                return "to";
            case SyntaxKind.TrueKeyword:
                return "true";
            case SyntaxKind.ElseKeyword:
                return "else";
            case SyntaxKind.FalseKeyword:
                return "false";
            case SyntaxKind.LetKeyword:
                return "let";
            case SyntaxKind.VarKeyword:
                return "var";
            case SyntaxKind.BangToken:
                return "!";
            case SyntaxKind.AmpersandAmpersandToken:
                return "&&";
            case SyntaxKind.PipePipeToken:
                return "||";
            case SyntaxKind.LessToken:
                return "<";
            case SyntaxKind.LessOrEqualToken:
                return "<=";
            case SyntaxKind.GreaterToken:
                return ">";
            case SyntaxKind.GreaterOrEqualToken:
                return ">=";
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
    public static IEnumerable<SyntaxKind> GetUnaryOperators()
    {
        var kinds = (SyntaxKind[])Enum.GetValues(typeof(SyntaxKind));

        foreach (var kind in kinds)
        {
            if(GetUnaryOperatorPrecedence(kind)>0)
                yield return kind;
        }
    }

    public static IEnumerable<SyntaxKind> GetBinaryOperators()
    {
        var kinds = (SyntaxKind[])Enum.GetValues(typeof(SyntaxKind));

        foreach (var kind in kinds)
        {
            if(GetBinaryOperatorPrecedence(kind)>0)
                yield return kind;
        }
    }
}