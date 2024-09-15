using System.Collections.Immutable;
using System.Xml.Xsl;
using Compiler.CodeAnalysis.Text;

namespace Compiler.CodeAnalysis.Syntax;

internal sealed class Parser
{
    private readonly DiagnosticBag _errorDiagnostics = new DiagnosticBag();
    private readonly SourceText _text;
    private readonly ImmutableArray<SyntaxToken> _tokens;
    private int _position;


    public DiagnosticBag ErrorDiagnostics => _errorDiagnostics;

    public Parser(SourceText text)
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

        _text = text;
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


    public CompilationUnitSyntax ParseCompilationUnit()
    {
        var statement = ParseStatement();
        var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
        return new CompilationUnitSyntax(statement, endOfFileToken);
    }

    private StatementSyntax ParseStatement()
    {
        if (Current.SyntaxKind == SyntaxKind.OpenBraceToken)
            return ParseBlockStatement();
        else if (Current.SyntaxKind == SyntaxKind.LetKeyword ||
                 Current.SyntaxKind == SyntaxKind.VarKeyword)
            return ParseVariableDeclaration();
        else if (Current.SyntaxKind == SyntaxKind.IfKeyword)
            return ParseIfStatement();
        else if (Current.SyntaxKind == SyntaxKind.WhileKeyword)
            return ParseWhileStatement();
        else if (Current.SyntaxKind == SyntaxKind.ForKeyword)
            return ParseForStatement();
        else
            return ParseExpressionStatement();
    }

    private StatementSyntax ParseForStatement()
    {
        var keywordToken = MatchToken(SyntaxKind.ForKeyword);
        var identifierToken = MatchToken(SyntaxKind.IdentifierToken);
        var equalsToken = MatchToken(SyntaxKind.EqualToken);
        var lowerBound = ParseExpression();
        var toKeyword = MatchToken(SyntaxKind.ToKeyword);
        var upperBound = ParseExpression();
        var body = ParseStatement();
        return new ForStatementSyntax(keywordToken,identifierToken, equalsToken, lowerBound, toKeyword,upperBound, body);

    }

    private StatementSyntax ParseWhileStatement()
    {
        var keywordToken = MatchToken(SyntaxKind.WhileKeyword);
        var condition = ParseExpression();
        var body = ParseStatement();
        return new WhileStatementSyntax(keywordToken, condition, body);
    }


    private StatementSyntax ParseVariableDeclaration()
    {
        var expected = Current.SyntaxKind == SyntaxKind.LetKeyword
            ? SyntaxKind.LetKeyword
            : SyntaxKind.VarKeyword;
        var keywordToken = MatchToken(expected);
        var identifier = MatchToken(SyntaxKind.IdentifierToken);
        var equals = MatchToken(SyntaxKind.EqualToken);
        var initializer = ParseExpression();
        return new VariableDeclarationSyntax(keywordToken, identifier, equals, initializer);
    }

    private ExpressionStatementSyntax ParseExpressionStatement()
    {
        var expression = ParseExpression();
        return new ExpressionStatementSyntax(expression);
    }

    private BlockStatementSyntax ParseBlockStatement()
    {
        var statements = ImmutableArray.CreateBuilder<StatementSyntax>();
        var openBraceToken = MatchToken(SyntaxKind.OpenBraceToken);
        while (Current.SyntaxKind != SyntaxKind.EndOfFileToken &&
               Current.SyntaxKind != SyntaxKind.CloseBraceToken)
        {
            var statement = ParseStatement();
            statements.Add(statement);
        }

        var closeraceToken = MatchToken(SyntaxKind.CloseBraceToken);

        return new BlockStatementSyntax(openBraceToken, statements.ToImmutable(), closeraceToken);
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

    private StatementSyntax ParseIfStatement()
    {
        var keywordToken = MatchToken(SyntaxKind.IfKeyword);
        var condition = ParseExpression();
        var statement = ParseStatement();
        var parseClause = ParseElseClause();
        return new IfStatementSyntax(keywordToken, condition, statement, parseClause);
    }

    private ElseClauseSyntax ParseElseClause()
    {
        if (Current.SyntaxKind != SyntaxKind.ElseKeyword)
            return null;
        var keyword = NextToken();
        var statement = ParseStatement();
        return new ElseClauseSyntax(keyword, statement);
    }

    private ExpressionSyntax ParseBooleanLiteral()
    {
        var isTrue = Current.SyntaxKind == SyntaxKind.TrueKeyword;
        var keywordToken = isTrue ? MatchToken(SyntaxKind.TrueKeyword) : MatchToken(SyntaxKind.FalseKeyword);
        return new LiteralExpressionSyntax(keywordToken, isTrue);
    }

    private ExpressionSyntax ParseNameToken()
    {
        var identifierToken = MatchToken(SyntaxKind.IdentifierToken);
        return new NameExpressionSyntax(identifierToken);
    }
}