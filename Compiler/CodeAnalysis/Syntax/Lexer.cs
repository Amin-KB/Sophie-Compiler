using System.Text;
using Compiler.CodeAnalysis.Symbols;
using Compiler.CodeAnalysis.Text;

namespace Compiler.CodeAnalysis.Syntax;

internal sealed class Lexer
{
    private DiagnosticBag _errorDiagnostics = new DiagnosticBag();
    private readonly SourceText _text;
    private int _position;
    public DiagnosticBag ErrorDiagnostics => _errorDiagnostics;
    private int _start;
    private SyntaxKind _kind;
    private object _value;

    public Lexer(SourceText text)
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


    /// <summary>
    /// Retrieves the next token from the input stream.
    /// </summary>
    /// <returns>A <see cref="SyntaxToken"/> representing the next token in the input stream.</returns>
    public SyntaxToken Lex()
    {
        _start = _position;
        _kind = SyntaxKind.BadToken;
        _value = null;

        switch (Current)
        {
            case ('\0'):
                _kind = SyntaxKind.EndOfFileToken;
                break;
            case ('+'):
                _kind = SyntaxKind.PlusToken;
                _position++;
                break;
            case ('-'):
                _kind = SyntaxKind.MinusToken;
                _position++;
                break;
            case ('*'):
                _kind = SyntaxKind.StarToken;
                _position++;
                break;
            case ('/'):
                _kind = SyntaxKind.SlashToken;
                _position++;
                break;
            case ('('):
                _kind = SyntaxKind.OpenParenthesisToken;
                _position++;
                break;
            case (')'):
                _kind = SyntaxKind.CloseParenthesisToken;
                _position++;
                break;
            case ('{'):
                _kind = SyntaxKind.OpenBraceToken;
                _position++;
                break;
            case ('}'):
                _kind = SyntaxKind.CloseBraceToken;
                _position++;
                break;
            case (','):
                _kind = SyntaxKind.CommaToken;
                _position++;
                break;
            case ('~'):
                _kind = SyntaxKind.TildeToken;
                _position++;
                break;
            case ('^'):
                _kind = SyntaxKind.HatToken;
                _position++;
                break;
            case ('&'):
                _position++;
                if (Current != '&')
                {
                    _kind = SyntaxKind.AmpersandToken;
                }
                else
                {
                    _kind = SyntaxKind.AmpersandAmpersandToken;
                    _position++;
                }
                break;
            case ('|'):
                _position++;
                if (Current != '|')
                {
                    _kind = SyntaxKind.PipeToken;
                }
                else
                {
                    _kind = SyntaxKind.PipePipeToken;
                    _position++;
                }
                break;
            case ('='):
                _position++;
                if (Current != '=')
                {
                    _kind = SyntaxKind.EqualToken;
                }
                else
                {
                    _kind = SyntaxKind.EqualEqualToken;
                    _position++;
                }
                break;

            case ('!'):
                _position++;
                if (Current != '=')
                {
                    _kind = SyntaxKind.BangToken;
                }

                else
                {
                    _kind = SyntaxKind.BangEqualToken;
                    _position++;
                }
                break;
            case '<':
                _position++;
                if (Current != '=')
                {
                    _kind = SyntaxKind.LessToken;
                }
                else
                {
                    _kind = SyntaxKind.LessOrEqualToken;
                    _position++;
                }
                break;
            case '>':
                _position++;
                if (Current != '=')
                {
                    _kind = SyntaxKind.GreaterToken;
                }
                else
                {
                    _kind = SyntaxKind.GreaterOrEqualToken;
                    _position++;
                }
                break;
            case '"':
                ReadString();
                break;
            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
                ReadNumberToken();
                break;
            case ' ':
            case '\t':
            case '\n':
            case '\r':
                ReadWhiteSpace();
                break;
            default:

                if (char.IsLetter(Current))
                {
                    ReadIdentifierOrKeyword();
                }
                else if (char.IsWhiteSpace(Current))
                {
                    ReadWhiteSpace();
                }
                else
                {
                    _errorDiagnostics.ReportBadCharacter(_position, Current);
                    _position++;
                }

                break;
        }

        var length = _position - _start;
        var text = SyntaxFact.GetText(_kind);
        if (text == null)
            text = _text.ToString(_start, length);
        return new SyntaxToken(_kind, _start, text, _value);
    }

    private void ReadString()
    {
        _position++;
        var stringBuilder = new StringBuilder();
        var done = false;
        while (!done)
        {
            switch (Current)
            {
                case '\0':
                case '\r':
                case '\n':
                    var span = new TextSpan(_start, 1);
                    _errorDiagnostics.ReportUnterminatedString(span);
                    done = true;
                    break;
                case '"':
                    if (LookAhead == '"')
                    {
                        stringBuilder.Append(Current);
                        _position+=2;
                    }
                    else
                    {
                        _position++;
                        done = true;
                    }
                       
                    break;
                default:
                    stringBuilder.Append(Current);
                    _position++;
                    break;
            }
        }
        _kind = SyntaxKind.StringToken;
        _value = stringBuilder.ToString();
    }

    private void ReadIdentifierOrKeyword()
    {
        while (char.IsLetter(Current))
            _position++;
        var length = _position - _start;
        var text = _text.ToString(_start, length);
        _kind = SyntaxFact.GetKeywordKind(text);
    }

    private void ReadWhiteSpace()
    {
        while (char.IsWhiteSpace(Current))
            _position++;
        _kind = SyntaxKind.WhiteSpaceToken;
    }

    private void ReadNumberToken()
    {
        while (char.IsDigit(Current))
            _position++;
        var length = _position - _start;
        var text = _text.ToString(_start, length);
        if (!int.TryParse(text, out var value))
            _errorDiagnostics.ReportInvalidNumber(new TextSpan(_start, length), text, TypeSymbol.Int);
        _value = value;
        _kind = SyntaxKind.NumberToken;
    }
}