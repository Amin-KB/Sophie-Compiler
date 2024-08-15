using Compiler.CodeAnalysis.Syntax;

namespace Sophie.Tests.SyntaxAnalysis.Syntax;

public class ParserTest
{
    [Theory]
    [MemberData(nameof(GetBinaryOperatorsPairData))]
    public void Parser_BinaryExpression_Precedences(SyntaxKind op1, SyntaxKind op2)
    {
        var op1Prec = SyntaxFact.GetBinaryOperatorPrecedence(op1);
        var op2Prec = SyntaxFact.GetBinaryOperatorPrecedence(op2);
        var op1Txt = SyntaxFact.GetText(op1);
        var op2Txt = SyntaxFact.GetText(op2);
        var text = $"a {op1Txt} b {op2Txt} c";
        var expression = SyntaxTree.Parse(text).Root;
        if (op1Prec >= op2Prec)
        {
            using (var e = new AssertingEnumerator(expression))
            {
                e.AssertNode(SyntaxKind.BinaryExpression);
                e.AssertNode(SyntaxKind.BinaryExpression);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.IdentifierToken, "a");
                e.AssertToken(op1, op1Txt);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.IdentifierToken, "b");
                e.AssertToken(op2, op2Txt);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.IdentifierToken, "c");
          

            }
        }
        else
        {
            using (var e = new AssertingEnumerator(expression))
            {
                e.AssertNode(SyntaxKind.BinaryExpression);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.IdentifierToken, "a");
                e.AssertToken(op1, op1Txt);
                e.AssertNode(SyntaxKind.BinaryExpression);

                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.IdentifierToken, "b");
                e.AssertToken(op2, op2Txt);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.IdentifierToken, "c");
            }
        }
    }
    
          [Theory]
        [MemberData(nameof(GetUnaryOperatorPairsData))]
        public void Parser_UnaryExpression_HonorsPrecedences(SyntaxKind op1, SyntaxKind op2)
        {
            var op1Prec = SyntaxFact.GetUnaryOperatorPrecedence(op1);
            var op2Prec = SyntaxFact.GetUnaryOperatorPrecedence(op2);
            var op1Txt = SyntaxFact.GetText(op1);
            var op2Txt = SyntaxFact.GetText(op2);
            var text = $"a {op1Txt} b {op2Txt} c";
            var expression = SyntaxTree.Parse(text).Root;
            if (op1Prec >= op2Prec)
            {
                using (var e = new AssertingEnumerator(expression))
                {
                    e.AssertNode(SyntaxKind.BinaryExpression);
                    e.AssertNode(SyntaxKind.BinaryExpression);
                    e.AssertNode(SyntaxKind.NameExpression);
                    e.AssertToken(SyntaxKind.IdentifierToken, "a");
                    e.AssertToken(op1, op1Txt);
                    e.AssertNode(SyntaxKind.NameExpression);
                    e.AssertToken(SyntaxKind.IdentifierToken, "b");
                    e.AssertToken(op2, op2Txt);
                    e.AssertNode(SyntaxKind.NameExpression);
                    e.AssertToken(SyntaxKind.IdentifierToken, "c");
          

                }
            }
            else
            {
                using (var e = new AssertingEnumerator(expression))
                {
                    e.AssertNode(SyntaxKind.BinaryExpression);
                    e.AssertNode(SyntaxKind.NameExpression);
                    e.AssertToken(SyntaxKind.IdentifierToken, "a");
                    e.AssertToken(op1, op1Txt);
                    e.AssertNode(SyntaxKind.BinaryExpression);

                    e.AssertNode(SyntaxKind.NameExpression);
                    e.AssertToken(SyntaxKind.IdentifierToken, "b");
                    e.AssertToken(op2, op2Txt);
                    e.AssertNode(SyntaxKind.NameExpression);
                    e.AssertToken(SyntaxKind.IdentifierToken, "c");
                }
            }
        }


    public static IEnumerable<object[]> GetBinaryOperatorsPairData()
    {
        foreach (var op1 in SyntaxFact.GetBinaryOperators())
        {
            foreach (var op2 in SyntaxFact.GetBinaryOperators())
            {
                yield return new object[] { op1, op2 };
            }
        }
    }
    public static IEnumerable<object[]> GetUnaryOperatorPairsData()
    {
        foreach (var unary in SyntaxFact.GetBinaryOperators())
        {
            foreach (var binary in SyntaxFact.GetUnaryOperators())
            {
                yield return new object[] { unary, binary };
            }
        }
    }
}