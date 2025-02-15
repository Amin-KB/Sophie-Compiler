using Compiler.CodeAnalysis;
using Compiler.CodeAnalysis.Symbols;
using Compiler.CodeAnalysis.Syntax;
using Compiler.CodeAnalysis.Text;

namespace Sophie_Compiler;

internal sealed class SophieREPL : REPL
{
    private Compilation _previous;
    private bool _showTree;
    private bool _showProgram;
    private readonly Dictionary<VariableSymbol, object> _variables = new Dictionary<VariableSymbol, object>();

    protected override void EvaluateMetaCommand(string input)
    {
        switch (input)
        {
            case "#showTree":
                _showTree = !_showTree;
                Console.WriteLine(_showTree ? "Show Parse Tree " : "Hide Parse Tree ");
                break;
            case "#showProgram":
                _showProgram = !_showProgram;
                Console.WriteLine(_showTree ? "Show bound Tree " : "Hide bound Tree ");
                break;
            case "#clr":
                Console.Clear();
                break;
            default:
                base.EvaluateMetaCommand(input);
                break;
        }
    }

    protected override bool EvaluateSubmission(string inputText)
    {
        var syntaxTree = SyntaxTree.Parse(inputText);

        var compilation = _previous == null
            ? new Compilation(syntaxTree)
            : _previous.ContinueWith(syntaxTree);


        var result = compilation.Evaluate(_variables);

        if (_showTree)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            syntaxTree.Root.WriteTo(Console.Out);
            Console.ResetColor();
        }

        if (_showProgram)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            compilation.EmitTree(Console.Out);
            Console.ResetColor();
        }

        if (!result.Diagnostics.Any())
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(result.Value);
            Console.ResetColor();

            _previous = compilation;
        }
        else
        {
            var text = syntaxTree.Text;
            foreach (var diagnostic in result.Diagnostics)
            {
                var lineIndex = text.GetLineIndex(diagnostic.TextSpan.Start);
                var line = text.Lines[lineIndex];
                var lineNumber = lineIndex + 1;

                var character = diagnostic.TextSpan.Start - line.Start + 1;
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"({lineNumber},{character}): ");
                Console.WriteLine(diagnostic);
                Console.ResetColor();
                var prefixSpan = TextSpan.FromBounds(line.Start, diagnostic.TextSpan.Start);
                var sufixSpan = TextSpan.FromBounds(diagnostic.TextSpan.End, line.End);
                var prefix = syntaxTree.Text.ToString(prefixSpan);
                var error = syntaxTree.Text.ToString(diagnostic.TextSpan);
                var suffix = syntaxTree.Text.ToString(sufixSpan);

                Console.Write("    ");
                Console.Write(prefix);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(error);
                Console.ResetColor();
                Console.WriteLine(suffix);
            }

            Console.ResetColor();
        }

        return false;
    }

    protected override void RenderLine(string line)
    {
        var tokens = SyntaxTree.ParseTokens(line);

        foreach (var token in tokens)
        {
            var isKeyword = token.SyntaxKind.ToString().EndsWith("Keyword");
            var isNumber = token.SyntaxKind == SyntaxKind.NumberToken;
            var isIdentifier = token.SyntaxKind == SyntaxKind.IdentifierToken;
            if (isKeyword)
                Console.ForegroundColor = ConsoleColor.Blue;
            else if (isIdentifier)
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
            else if (isNumber)
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            else
                Console.ForegroundColor = ConsoleColor.DarkGray;


            Console.Write(token.Text);
            Console.ResetColor();
        }
    }

    protected override bool IsCompleteSubmission(string text)
    {
        if (string.IsNullOrEmpty(text))
            return true;
        var lastTwoLinesAreBlank = text.Split(Environment.NewLine).Reverse()
                                     .TakeWhile(s => string.IsNullOrEmpty(s))
                                     .Take(2)
                                     .Count() == 2;
        if (lastTwoLinesAreBlank)
            return true;

        var syntaxTree = SyntaxTree.Parse(text);

        if (syntaxTree.Root.Statement.GetLastToken().IsMissing)
            return false;

        return true;
    }


}