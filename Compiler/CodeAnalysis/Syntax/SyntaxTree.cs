using System.Collections.Immutable;
using Compiler.CodeAnalysis.Text;

namespace Compiler.CodeAnalysis.Syntax;

public sealed class SyntaxTree
{
    public SourceText Text { get; }
    public ImmutableArray<Diagnostic> Diagnostics { get; }
    public CompilationUnitSyntax Root { get; }

    public SyntaxToken EndOfFileToken { get; }


    private SyntaxTree(SourceText text)
    {
        var parser = new Parser(text);
        Root =  parser.ParseCompilationUnit();
        var diagnostics = parser.ErrorDiagnostics.ToImmutableArray();
        Text = text;
        Diagnostics = diagnostics;
      
    }
    public static SyntaxTree Parse(string text)
    {
        var sourceText= SourceText.From(text);
        return Parse(sourceText);
     
    }
    public static SyntaxTree Parse(SourceText text)
    {
        return new SyntaxTree(text);
    }
    public static IEnumerable<SyntaxToken> ParseToken(string text)
    {
        var sourceText= SourceText.From(text);
        return ParseToken(sourceText);
    }
    public static IEnumerable<SyntaxToken> ParseToken(SourceText text)
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