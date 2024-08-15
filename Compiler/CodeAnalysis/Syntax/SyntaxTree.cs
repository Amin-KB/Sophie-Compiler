using System.Collections.Immutable;

namespace Compiler.CodeAnalysis.Syntax;

public sealed class SyntaxTree
{
    public ExpressionSyntax Root { get; }
    public SyntaxToken EndOfFileToken { get; }
    public ImmutableArray<Diagnostic> Diagnostics { get; }

    public SyntaxTree(ImmutableArray<Diagnostic> diagnostics, ExpressionSyntax root, SyntaxToken endOfFileToken)
    {
        Root = root;
        EndOfFileToken = endOfFileToken;
        Diagnostics = diagnostics;
    }

    public static SyntaxTree Parse(string text)
    {
        var parser = new Parser(text);
        return parser.Parse();
    }

    public static IEnumerable<SyntaxToken> ParseToken(string text)
    {
        var lexer = new Lexer(text);
        while (true)
        {
            var token = lexer.Lex();
            if (token.SyntaxKind == SyntaxKind.EndOfFileToken)
                break;

            yield return token;
        }
    }
}