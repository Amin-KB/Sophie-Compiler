using System.Runtime.InteropServices;
using Compiler.CodeAnalysis.Syntax;
using Compiler.CodeAnalysis.Binding;
using Compiler.CodeAnalysis;


Run();
static void Run()
{
    var variables = new Dictionary<VariableSymbol, object>();
    while (true)
    {
        Console.Write("> ");
        var line = Console.ReadLine();
        if (string.IsNullOrEmpty(line))
            return;
        var syntaxTree = SyntaxTree.Parse(line);
        var compilation = new Compilation(syntaxTree);
        var result = compilation.Evaluate(variables);
        var _diagnostics = result.Diagnostics;
        var color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkGray;
       Print(syntaxTree.Root);
        
        Console.ForegroundColor = color;
        if (!_diagnostics.Any())
        {
        
           
            Console.WriteLine(result.Value); 
        }
        else
        {
            
            foreach (var diagnostic in _diagnostics)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(diagnostic);
                Console.ResetColor();
                var prefix = line.Substring(0, diagnostic.TextSpan.Start);
                var error = line.Substring(diagnostic.TextSpan.Start,diagnostic.TextSpan.Length);
                var suffix = line.Substring(diagnostic.TextSpan.End);

                Console.Write("    ");
                Console.Write(prefix);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(error);
                Console.ResetColor();
                Console.WriteLine(suffix);
                
            }
            
            Console.ResetColor(); 
        }
       
    }
}

static void Print(SyntaxNode node, string indent = "",bool isLast=false)
{
    var marker = isLast ? "\u2514\u2500\u2500" : "\u251c\u2500\u2500";
    Console.Write(indent);
    Console.Write(marker);
    Console.Write(node.SyntaxKind);
    if (node is SyntaxToken t && t.Value != null)
    {
        Console.Write(" ");
        Console.Write(t.Value);
    }

    Console.WriteLine();
    indent += isLast ? "   " : "\u2502  ";
    var lastChild = node.GetChildren().LastOrDefault();
    foreach (var child in node.GetChildren())
    {
        Print(child, indent,child==lastChild);
    }
}