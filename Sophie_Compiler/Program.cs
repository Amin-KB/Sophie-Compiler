using System.Runtime.InteropServices;
using Sophie_Compiler.LexerAndParser;
using Sophie_Compiler.LexerAndParser.Binding;

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
        var binder = new Binder();
        var boundExpression = binder.BindExpression(syntaxTree.Root);
        var _diagnostics = syntaxTree.Diagnostics.Concat(binder.Diagnostics).ToArray();
        var color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkGray;
       Print(syntaxTree.Root);
        
        Console.ForegroundColor = color;
        if (!syntaxTree.Diagnostics.Any())
        {
            var e = new Evaluator(boundExpression);
            var result = e.Evaluete();
            Console.WriteLine(result); 
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var error in syntaxTree.Diagnostics) 
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