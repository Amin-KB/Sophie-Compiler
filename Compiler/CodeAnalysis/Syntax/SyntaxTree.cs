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
    public static ImmutableArray<SyntaxToken> ParseTokens(string text)
    {
        var sourceText= SourceText.From(text);
        return ParseTokens(sourceText);
    }
    public static ImmutableArray<SyntaxToken> ParseTokens(string text, out ImmutableArray<Diagnostic> diagnostics)
    {
        var sourceText = SourceText.From(text);
        return ParseTokens(sourceText,out diagnostics);
    }
    public static ImmutableArray<SyntaxToken> ParseTokens(SourceText text)
    {
        return ParseTokens(text, out _);
    }
    public static ImmutableArray<SyntaxToken> ParseTokens(SourceText text,out ImmutableArray<Diagnostic> diagnostics)
    {
        IEnumerable<SyntaxToken> LexTokens(Lexer lexer)
        {
            while (true)
            {
                var token = lexer.Lex();
                if (token.SyntaxKind == SyntaxKind.EndOfFileToken)
                    break;

                yield return token;
            }
        }
        var lexer = new Lexer(text);
        var result= LexTokens(lexer).ToImmutableArray();
        diagnostics=lexer.ErrorDiagnostics.ToImmutableArray();
        return result;
    
    }
}