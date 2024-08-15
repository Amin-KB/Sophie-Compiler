using Compiler.CodeAnalysis.Syntax;

namespace Sophie.Tests.SyntaxAnalysis.Syntax;

public class LexerTest
{

    [Fact]
 
    public void Lexer_Tests_AllToken()
    {
        var tokenKinds = Enum.GetValues(typeof(SyntaxKind))
                         .Cast<SyntaxKind>()
                         .Where(k=>k.ToString().EndsWith("Keyword") ||
                                    k.ToString().EndsWith("Token"))
                          .ToList();

        var testedTokenKinds=GetTokens().Concat(GetSeparators()).Select(t=>t.kind);
        var untestedToknKinds=new SortedSet<SyntaxKind>(tokenKinds);
        untestedToknKinds.Remove(SyntaxKind.BadToken);
        untestedToknKinds.Remove(SyntaxKind.EndOfFileToken);
        untestedToknKinds.ExceptWith(testedTokenKinds);
        Assert.Empty(untestedToknKinds);
    
    }

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

    [Theory]
    [MemberData(nameof(GetPairsTokenWithSeparatorData))]
    public void Lexer_Lex_PairsTokenWithSeparators(SyntaxKind aKind, string aText,
        SyntaxKind separatorKind, string separatorText,
        SyntaxKind bKind, string bText)
    {
        var text = aText + separatorText + bText;
        var tokens = SyntaxTree.ParseToken(text).ToArray();

        Assert.Equal(3, tokens.Length);
        Assert.Equal(tokens[0].SyntaxKind, aKind);
        Assert.Equal(tokens[0].Text, aText);
        Assert.Equal(tokens[1].SyntaxKind, separatorKind);
        Assert.Equal(tokens[1].Text, separatorText);
        Assert.Equal(tokens[2].SyntaxKind, bKind);
        Assert.Equal(tokens[2].Text, bText);

   
    }

    public static IEnumerable<object[]> GetTokenData()
    {
        foreach (var token in GetTokens().Concat(GetSeparators()))
            yield return new object[] { token.kind, token.text };
    }

    public static IEnumerable<object[]> GetPairsTokenData()
    {
        foreach (var token in GetPairsTokens())
            yield return new object[] { token.aKind, token.aText, token.bKind, token.bText };
    }

    public static IEnumerable<object[]> GetPairsTokenWithSeparatorData()
    {
        foreach (var token in GetPairsWithSeparatorsTokens())
            yield return new object[]
            {
                token.aKind, token.aText,
                token.seperatorKind, token.separatorText,
                token.bKind, token.bText
            };
    }

    public static IEnumerable<(SyntaxKind kind, string text)> GetSeparators()
    {
        return new[]
        {
            (SyntaxKind.WhiteSpaceToken, " "),
            (SyntaxKind.WhiteSpaceToken, "   "),
            (SyntaxKind.WhiteSpaceToken, "\r"),
            (SyntaxKind.WhiteSpaceToken, "\n"),
            (SyntaxKind.WhiteSpaceToken, "\r\n"),
        };
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
        var fixedTokens=Enum.GetValues(typeof(SyntaxKind))
                            .Cast<SyntaxKind>()
                            .Select(k=>(kind:k,text:SyntaxFact.GetText(k)))
                            .Where(t=>t.text!=null);
        var dynamicTokens= new[]
        {
                (SyntaxKind.NumberToken, "1"),
            (SyntaxKind.NumberToken, "123"),
        
                (SyntaxKind.IdentifierToken, "a"),

            (SyntaxKind.IdentifierToken, "abc"),
            //(SyntaxKind.PlusToken, "+"),
            //(SyntaxKind.MinusToken, "-"),
            //(SyntaxKind.StarToken, "*"),
            //(SyntaxKind.SlashToken, "/"),

            //(SyntaxKind.BangToken, "!"),
            //(SyntaxKind.AmpersandAmperSandToken, "&&"),
            //(SyntaxKind.PipePipeToken, "||"),

            //(SyntaxKind.EqualToken, "="),
            //(SyntaxKind.EqualEqualToken, "=="),
            //(SyntaxKind.BangEqualToken, "!="),
            //(SyntaxKind.OpenParenthesisToken, "("),
            //(SyntaxKind.CloseParenthesisToken, ")"),

        
            //(SyntaxKind.TrueKeyword, "true"),
            //(SyntaxKind.FalseKeyword, "false"),
        };
        return fixedTokens.Concat(dynamicTokens);
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

    public static IEnumerable<(SyntaxKind aKind, string aText,
        SyntaxKind seperatorKind, string separatorText,
        SyntaxKind bKind, string bText)> GetPairsWithSeparatorsTokens()
    {
        foreach (var a in GetTokens())
        {
            foreach (var b in GetTokens())
            {
                if (!RequiresSeparator(a.kind, b.kind))
                {
                    foreach (var s in GetSeparators())
                        yield return (a.kind, a.text, s.kind, s.text, b.kind, b.text);
                }
            }
        }
    }
}