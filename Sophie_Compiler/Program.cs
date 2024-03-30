using System.Runtime.InteropServices;
using Compiler.CodeAnalysis.Syntax;
using Compiler.CodeAnalysis.Binding;
using Compiler.CodeAnalysis;


Run();
static void Run()
{
    while (true)
    {
        Console.Write("> ");
        var line = Console.ReadLine();
        if (string.IsNullOrEmpty(line))
            return;
        var syntaxTree = SyntaxTree.Parse(line);
        var compilation = new Compilation(syntaxTree);
        var result = compilation.Evaluate();
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
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var error in _diagnostics) 
                Console.WriteLine(error);
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