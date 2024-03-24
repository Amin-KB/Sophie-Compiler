namespace Sophie_Compiler.LexerAndParser;

public class Parser
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
    private SyntaxToken Match(SyntaxKind kind)
    {
        if (Current.SyntaxKind == kind)
            return NextToken();
        
        _errorDiagnostics.Add($"EEROR: Unexpected token <{Current.SyntaxKind}>, expected <{kind}>");
        return new SyntaxToken(kind, Current.Position, null, null);
    }
    public SyntaxTree Parse()
    {
        var expression= ParseExpression();
        var endOfFileToken = Match(SyntaxKind.EndOfFileToken);
        return new SyntaxTree(_errorDiagnostics, expression, endOfFileToken);
    }
    public ExpressionSyntax ParseExpression()
    {
        var left = ParsePrimaryExpression();
        while (Current.SyntaxKind == SyntaxKind.PlusToken ||
               Current.SyntaxKind == SyntaxKind.MinusToken||
               Current.SyntaxKind == SyntaxKind.StarToken||
               Current.SyntaxKind == SyntaxKind.SlashToken
               )
        {
            var operationToken = NextToken();
            var right = ParsePrimaryExpression();
            left = new BinaryExpressionSyntax(left, operationToken, right);
        }

        return left;
    }
    private ExpressionSyntax ParsePrimaryExpression()
    {
        var numberToken = Match(SyntaxKind.NumberToken);
        return new NumberExpressionSyntax(numberToken);
    }
}