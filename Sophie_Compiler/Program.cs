﻿using System.Runtime.InteropServices;
using System.Text;
using Compiler.CodeAnalysis.Syntax;
using Compiler.CodeAnalysis.Binding;
using Compiler.CodeAnalysis;
using Compiler.CodeAnalysis.Text;

Run();

static void Run()
{
    var showTree = false;
    var variables = new Dictionary<VariableSymbol, object>();
    var textBuilder = new StringBuilder();
    while (true)
    {
        if (textBuilder.Length == 0)
            Console.Write("> ");
        else
            Console.Write("| ");

        var input = Console.ReadLine();
        var isBlank = string.IsNullOrEmpty(input);

        if (textBuilder.Length == 0)
        {
            if (isBlank)
            {
                break;
            }
            else if (input == "#showTree")
            {
                showTree = !showTree;
                Console.Write(showTree ? "Show Parse Tree " : "Hide Parse Tree ");
                continue;
            }
            else if (input == "#clr")
            {
                Console.Clear();
                continue;
            }
        }

        textBuilder.AppendLine(input);
        var inputText = textBuilder.ToString();
        var syntaxTree = SyntaxTree.Parse(inputText);
        if (!isBlank && syntaxTree.Diagnostics.Any())
            continue;
        var compilation = new Compilation(syntaxTree);
        var result = compilation.Evaluate(variables);

        if (showTree)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            syntaxTree.Root.WriteTo(Console.Out);
            Console.ResetColor();
        }

        if (!result.Diagnostics.Any())
        {
            Console.WriteLine(result.Value);
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
                var prefixSpan=TextSpan.FromBounds(line.Start,diagnostic.TextSpan.Start);
                var sufixSpan=TextSpan.FromBounds(diagnostic.TextSpan.End,line.End);
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
        textBuilder.Clear();
    }
}