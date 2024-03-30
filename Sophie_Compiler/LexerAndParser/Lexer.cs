﻿namespace Sophie_Compiler.LexerAndParser;

internal sealed class Lexer
{
    private readonly string _text;
    private int _position;
  

    public Lexer(string text)
    {
        _text = text;
    }

    private char Current
    {
        get
        {
            if (_position >= _text.Length)
                return '\0';
            return _text[_position];
        }
    }

    
    private List<string> _errorDiagnostics = new List<string>();
    public IEnumerable<string> ErrorDiagnostics => _errorDiagnostics;
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
                _errorDiagnostics.Add($"ERROR: unable to parse '{text}' as number");
   
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
        }
     
        _errorDiagnostics.Add($"ERROR: bad character input '{Current}'");
        return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
    }
}