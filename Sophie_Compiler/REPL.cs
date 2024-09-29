using System.Text;
using Compiler.CodeAnalysis;
using Compiler.CodeAnalysis.Syntax;
using Compiler.CodeAnalysis.Text;

namespace Sophie_Compiler;

internal abstract class REPL
{
    private StringBuilder _textBuilder=new StringBuilder();
    private Compilation _previous;
    private bool _showTree;
    private bool _showProgram;
    private Dictionary<VariableSymbol, object> _variables;

    public void Run()
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            if (_textBuilder.Length == 0)
                Console.Write("> ");
            else
                Console.Write("| ");
            Console.ResetColor();
            var input = Console.ReadLine();
            var isBlank = string.IsNullOrEmpty(input);

            if (_textBuilder.Length == 0)
            {
                if (isBlank)
                {
                    break;
                }
                else if (input.StartsWith("#"))
                {
                    EvaluateMetaCommand(input);
                    continue;
                }
            }

            _textBuilder.AppendLine(input);
            var inputText = _textBuilder.ToString();
            if (!IsCompleteSubmission(inputText))
                continue;
            if (EvaluateSubmission(inputText)) continue;

            _textBuilder.Clear();
        }
    }

    protected virtual void EvaluateMetaCommand(string input)
    {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Invalid command: {input}.");
            Console.ResetColor();
    }

    protected abstract bool EvaluateSubmission(string inputText);


    protected abstract bool IsCompleteSubmission(string text);

}