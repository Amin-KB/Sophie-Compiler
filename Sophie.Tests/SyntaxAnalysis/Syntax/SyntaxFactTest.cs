using Compiler.CodeAnalysis.Syntax;

namespace Sophie.Tests.SyntaxAnalysis.Syntax;

public class SyntaxFactTest
{
    [Theory]
    [MemberData(nameof(GetSyntaxKindData))]
    public void SyntaxFact_GetText_RoundTrips(SyntaxKind kind)
    {
        var text = SyntaxFact.GetText(kind);
        if(text==null)
            return;

        var tokens= SyntaxTree.ParseTokens(text);
        var token = Assert.Single(tokens);
        Assert.Equal(kind,token.SyntaxKind);
        Assert.Equal(text,token.Text);
    }
   
    public static IEnumerable<object[]> GetSyntaxKindData()
    {
        var values = (SyntaxKind[])Enum.GetValues((typeof(SyntaxKind)));
        foreach (var value in values)
            yield return new object[] { value };
    }
}