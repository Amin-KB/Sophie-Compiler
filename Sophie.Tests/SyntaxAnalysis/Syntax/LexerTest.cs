using Compiler.CodeAnalysis.Syntax;

namespace Sophie.Tests.SyntaxAnalysis.Syntax;

public class LexerTest
{
    [Theory]
    [MemberData(nameof(GetTokenData))]
    public void Lexer_Lex_Token(SyntaxKind kind, string text)
    {
        var tokens = SyntaxTree.ParseToken(text);

        var token = Assert.Single(tokens);
        Assert.Equal(kind, token.SyntaxKind);
        Assert.Equal(text, token.Text);
    }

    [Theory]
    [MemberData(nameof(GetPairsTokenData))]
    public void Lexer_Lex_PairsToken(SyntaxKind aKind, string aText,
        SyntaxKind bKind, string bText)
    {
        var text = aText + bText;
        var tokens = SyntaxTree.ParseToken(text).ToArray();

        Assert.Equal(2, tokens.Length);
        Assert.Equal(tokens[0].SyntaxKind, aKind);
        Assert.Equal(tokens[0].Text, aText);

        Assert.Equal(tokens[1].SyntaxKind, bKind);
        Assert.Equal(tokens[1].Text, bText);
    }

    public static IEnumerable<object[]> GetTokenData()
    {
        foreach (var token in GetTokens())
            yield return new object[] { token.kind, token.text };
    }

    public static IEnumerable<object[]> GetPairsTokenData()
    {
        foreach (var token in GetPairsTokens())
            yield return new object[] { token.aKind, token.aText, token.bKind, token.bText };
    }

    private static bool RequiresSeparator(SyntaxKind aKind, SyntaxKind bKind)
    {
        var aIsKeyword = aKind.ToString().EndsWith("Keyword");
        var bIsKeyword = bKind.ToString().EndsWith("Keyword");

        if (aKind == SyntaxKind.IdentifierToken && bKind == SyntaxKind.IdentifierToken)
            return true;
        if (aIsKeyword && bIsKeyword)
            return true;
        if (aKind == SyntaxKind.IdentifierToken && bIsKeyword)
            return true;
        if (bKind == SyntaxKind.IdentifierToken && aIsKeyword)
            return true;
        if (aKind == SyntaxKind.NumberToken && bKind == SyntaxKind.NumberToken)
            return true;
        if (aKind == SyntaxKind.BangToken && bKind == SyntaxKind.EqualToken)
            return true;
        if (aKind == SyntaxKind.BangToken && bKind == SyntaxKind.EqualEqualToken)
            return true;
        if (aKind == SyntaxKind.EqualToken && bKind == SyntaxKind.EqualToken)
            return true;
        if (aKind == SyntaxKind.EqualToken && bKind == SyntaxKind.EqualEqualToken)
            return true;
      
        return false;
    }

    public static IEnumerable<(SyntaxKind kind, string text)> GetTokens()
    {
        return new[]
        {
            // (SyntaxKind.WhiteSpaceToken, " "),
            // (SyntaxKind.WhiteSpaceToken, "   "),
            // (SyntaxKind.WhiteSpaceToken, "\r"),
            // (SyntaxKind.WhiteSpaceToken, "\n"),
            // (SyntaxKind.WhiteSpaceToken, "\r\n"),

            (SyntaxKind.NumberToken, "123"),
            (SyntaxKind.NumberToken, "1"),

            (SyntaxKind.PlusToken, "+"),
            (SyntaxKind.MinusToken, "-"),
            (SyntaxKind.StarToken, "*"),
            (SyntaxKind.SlashToken, "/"),

            (SyntaxKind.BangToken, "!"),
            (SyntaxKind.AmpersandAmperSandToken, "&&"),
            (SyntaxKind.PipePipeToken, "||"),

            (SyntaxKind.EqualToken, "="),
            (SyntaxKind.EqualEqualToken, "=="),
            (SyntaxKind.BangEqualToken, "!="),
            (SyntaxKind.OpenParenthesisToken, "("),
            (SyntaxKind.CloseParenthesisToken, ")"),

            (SyntaxKind.IdentifierToken, "a"),
            (SyntaxKind.IdentifierToken, "abc"),
            (SyntaxKind.TrueKeyword, "true"),
            (SyntaxKind.FalseKeyword, "false"),
        };
    }

    public static IEnumerable<(SyntaxKind aKind, string aText, SyntaxKind bKind, string bText)> GetPairsTokens()
    {
        foreach (var a in GetTokens())
        {
         
                foreach (var b in GetTokens())
                {
                    if (!RequiresSeparator(a.kind, b.kind))
                        yield return (a.kind, a.text, b.kind, b.text);
                }    
        
            
        }
    }
}