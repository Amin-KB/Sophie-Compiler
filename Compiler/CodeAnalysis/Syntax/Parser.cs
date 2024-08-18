using System.Collections.Immutable;
using System.Xml.Xsl;

namespace Compiler.CodeAnalysis.Syntax;

internal sealed class Parser
{
    private readonly DiagnosticBag _errorDiagnostics = new DiagnosticBag();
    private readonly ImmutableArray<SyntaxToken> _tokens;
    private int _position; 
 
    public DiagnosticBag ErrorDiagnostics => _errorDiagnostics;

    public Parser(string text)
    {
        var tokens = new List<SyntaxToken>();
        var lexer = new Lexer(text);
        SyntaxToken token;
        do
        {
            token = lexer.Lex();
            if (token.SyntaxKind != SyntaxKind.WhiteSpaceToken &&
                token.SyntaxKind != SyntaxKind.BadToken)
            {
                tokens.Add(token);
            }
        } while (token.SyntaxKind != SyntaxKind.EndOfFileToken);

        _tokens = tokens.ToImmutableArray();
        _errorDiagnostics.AddRange(lexer.ErrorDiagnostics);
    }

    private SyntaxToken Peek(int offset)
    {
        var index = _position + offset;
        if (index >= _tokens.Length)
            return _tokens[_tokens.Length - 1];
        return _tokens[index];
    }

    private SyntaxToken Current => Peek(0);

    private SyntaxToken NextToken()
    {
        var current = Current;
        _position++;
        return current;
    }

    private SyntaxToken MatchToken(SyntaxKind kind)
    {
        if (Current.SyntaxKind == kind)
            return NextToken();

        _errorDiagnostics.ReportUnexpectedToken(Current.Span, Current.SyntaxKind, kind);
        return new SyntaxToken(kind, Current.Position, null, null);
    }

    private ExpressionSyntax ParseExpression()
    {
        return ParseAssignmentExpression();
    }

    private ExpressionSyntax ParseAssignmentExpression()
    {
        if (Peek(0).SyntaxKind == SyntaxKind.IdentifierToken &&
            Peek(1).SyntaxKind == SyntaxKind.EqualToken)
        {
            var identifierToken = NextToken();
            var operatorToken = NextToken();
            var right = ParseAssignmentExpression();
            return new AssignmentExpressionSyntax(identifierToken, operatorToken, right);
        }

        return ParseBinaryExpression();
    }

    private ExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0)
    {
        ExpressionSyntax left;
        var unaryOperatorPrecedence = Current.SyntaxKind.GetUnaryOperatorPrecedence();
        if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
        {
            var operatorToken = NextToken();
            var operand = ParseBinaryExpression(unaryOperatorPrecedence);
            left = new UnaryExpressionSyntax(operatorToken, operand);
        }
        else
        {
            left = ParsePrimaryExpression();
        }

        while (true)
        {
            var precedence = Current.SyntaxKind.GetBinaryOperatorPrecedence();
            if (precedence == 0 || precedence <= parentPrecedence)
                break;
            var operatorToken = NextToken();
            var right = ParseBinaryExpression(precedence);
            left = new BinaryExpressionSyntax(left, operatorToken, right);
        }

        return left;
    }


    public SyntaxTree Parse()
    {
        var expression = ParseExpression();
        var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
        return new SyntaxTree(_errorDiagnostics.ToImmutableArray(), expression, endOfFileToken);
    }


    private ExpressionSyntax ParsePrimaryExpression()
    {
        switch (Current.SyntaxKind)
        {
            case SyntaxKind.OpenParenthesisToken:
                return ParseParenthesizedExpression();
            case SyntaxKind.TrueKeyword:
            case SyntaxKind.FalseKeyword:
                return ParseBooleanLiteral();
            case SyntaxKind.NumberToken:
                return ParseNumberExpression();
            case SyntaxKind.IdentifierToken:
            default:
                return ParseNameToken();




        }
    }

    private ExpressionSyntax ParseNumberExpression()
    {
        var numberToken = MatchToken(SyntaxKind.NumberToken);
        return new LiteralExpressionSyntax(numberToken);
    }

    private ExpressionSyntax ParseParenthesizedExpression()
    {
        var left = MatchToken(SyntaxKind.OpenParenthesisToken);
        var expression = ParseExpression();
        var right = MatchToken(SyntaxKind.CloseParenthesisToken);
        return new ParenthesizedExpressionSyntax(left, expression, right);
    }

    private ExpressionSyntax ParseBooleanLiteral()
    {
        var isTrue= Current.SyntaxKind == SyntaxKind.TrueKeyword;
        var keywordToken = isTrue ? MatchToken(SyntaxKind.TrueKeyword) : MatchToken(SyntaxKind.FalseKeyword);   
        return new LiteralExpressionSyntax(keywordToken, isTrue);
    }

    private ExpressionSyntax ParseNameToken()
    {
        var identifierToken = MatchToken(SyntaxKind.IdentifierToken); ;
        return new NameExpressionSyntax(identifierToken);
    }
}