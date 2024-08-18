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
        syntaxTree.Root.WriteTo(Console.Out);
        
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

