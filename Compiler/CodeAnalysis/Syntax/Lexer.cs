namespace Compiler.CodeAnalysis.Syntax;

internal sealed class Lexer
{
    private readonly string _text;
    private int _position;
  

    public Lexer(string text)
    {
        _text = text;
    }

    private char Current => Peek(0);
    private char LookAhead => Peek(1);
  
    private char Peek(int offset)
    {
        var index = _position + offset;
            if (index >= _text.Length)
                return '\0';
            return _text[index];
        
    }
    
    private DiagnosticBag _errorDiagnostics = new DiagnosticBag();
    public DiagnosticBag ErrorDiagnostics => _errorDiagnostics;
    private void Next()
    {
        _position++;
    }

    /// <summary>
    /// Retrieves the next token from the input stream.
    /// </summary>
    /// <returns>A <see cref="SyntaxToken"/> representing the next token in the input stream.</returns>
    public SyntaxToken NextToken()
    {
        if (_position >= _text.Length)
            return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);

        if (char.IsDigit(Current))
        {
            var start = _position;

            while (char.IsDigit(Current))
                Next();
            var length = _position - start;
            var text = _text.Substring(start, length);
            if (!int.TryParse(text, out var value))
                _errorDiagnostics.ReportInvalidNumber(new TextSpan(start,length),_text,typeof(int));
   
            return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
        }

        if (char.IsWhiteSpace(Current))
        {
            var start = _position;

            while (char.IsWhiteSpace(Current))
                Next();
            var length = _position - start;
            var text = _text.Substring(start, length);
            return new SyntaxToken(SyntaxKind.WhiteSpaceToken, start, text, null);
        }

        if (char.IsLetter(Current))
        {
            var start = _position;
            while (char.IsLetter(Current))
           Next();
            var length = _position - start;
            var text = _text.Substring(start, length);
            var kind = SyntaxFact.GetKeywordKind(text);
            return new SyntaxToken(kind, start, text, null);
        }
        switch (Current)
        {
            case('+'):
                return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
            case('-'):
                return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
            case('*'):
                return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
            case('/'):
                return new SyntaxToken(SyntaxKind.PlusToken, _position++, "/", null);
            case('('):
                return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null);
            case(')'):
                return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null);
        
            case('&'):
                if (LookAhead == '&')
                    return new SyntaxToken(SyntaxKind.AmpersandAmperSandToken, _position += 2, "&&", null);
                break;
            case('|'):
                if (LookAhead == '|')
                    return new SyntaxToken(SyntaxKind.PipePipeToken, _position += 2, "||", null);
                break;
            case('='):
                if (LookAhead == '=')
                    return new SyntaxToken(SyntaxKind.EqualEqualToken, _position += 2, "==", null);
                break;
            case('!'):
                if (LookAhead == '=')
                    return new SyntaxToken(SyntaxKind.BangEqualToken, _position += 2, "!=", null);
                else
                    return new SyntaxToken(SyntaxKind.BangToken, _position++, "!", null);
                break;
            
        }
     
        _errorDiagnostics.ReportBadCharacter(_position,Current);
        return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
    }
}