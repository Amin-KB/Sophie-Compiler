namespace Sophie_Compiler.LexerAndParser;

internal sealed class Parser
{
    private readonly SyntaxToken[] _tokens;
    private int _position;
    private List<string> _errorDiagnostics = new List<string>();
    public IEnumerable<string> ErrorDiagnostics => _errorDiagnostics;
    public Parser(string text)
    {
        var tokens = new List<SyntaxToken>();
        var lexer = new Lexer(text);
        SyntaxToken token;
        do
        {
            token = lexer.NextToken();
            if (token.SyntaxKind != SyntaxKind.WhiteSpaceToken &&
                token.SyntaxKind != SyntaxKind.BadToken)
            {
                tokens.Add(token);
            }
        } while (token.SyntaxKind != SyntaxKind.EndOfFileToken);

        _tokens = tokens.ToArray();
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
        
        _errorDiagnostics.Add($"EEROR: Unexpected token <{Current.SyntaxKind}>, expected <{kind}>");
        return new SyntaxToken(kind, Current.Position, null, null);
    }

    private ExpressionSyntax ParseExpression(int parentPrecedence = 0)
    {
        ExpressionSyntax left;
        var unaryOperatorPrecedence = Current.SyntaxKind.GetUnaryOperatorPrecedence();
        if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
        {
            var operatorToken = NextToken();
            var operand = ParseExpression(unaryOperatorPrecedence);
            left = new UnaryExpressionSyntax(operatorToken, operand);
        }
        else
        {
            left = ParsePrimaryExpression();
        }
        while (true)
        {
            var precedence = Current.SyntaxKind.GetBinaryOperatorPrecedence();
            if(precedence==0 || precedence<=parentPrecedence)
                break;
            var operatorToken = NextToken();
            var right = ParseExpression(precedence);
            left = new BinaryExpressionSyntax(left, operatorToken, right);
        }

        return left;
    }

 
    
    public SyntaxTree Parse()
    {
        var expression= ParseExpression();
        var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
        return new SyntaxTree(_errorDiagnostics, expression, endOfFileToken);
    }
  

    private ExpressionSyntax ParsePrimaryExpression()
    {
        if (Current.SyntaxKind == SyntaxKind.OpenParenthesisToken)
        {
            var left = NextToken();
            var expression = ParseExpression();
            var right = MatchToken(SyntaxKind.CloseParenthesisToken);
            return new ParenthesizedExpressionSyntax(left, expression, right);
        }
        var numberToken = MatchToken(SyntaxKind.NumberToken);
        return new LiteralExpressionSyntax(numberToken);
    }
}