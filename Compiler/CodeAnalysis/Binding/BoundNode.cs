using System.Data;
using System.Reflection;

namespace Compiler.CodeAnalysis.Binding;

public abstract class BoundNode
{
    public abstract BoundNodeKind Kind { get; }

    public IEnumerable<BoundNode> GetChildren()
    {
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            if (typeof(BoundNode).IsAssignableFrom(property.PropertyType))
            {
                var child = (BoundNode)property.GetValue(this);
                if (child != null)
                    yield return child;
            }
            else if (typeof(IEnumerable<BoundNode>).IsAssignableFrom(property.PropertyType))
            {
                var children = (IEnumerable<BoundNode>)property.GetValue(this);
                foreach (var child in children)
                {
                    if (child != null)
                        yield return child;
                }
            }
        }
    }

    private IEnumerable<(string name, object value)> GetProperites()
    {
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            if (property.Name == nameof(Kind) ||
                 property.Name ==nameof(BoundBinaryExpression.Op))
                continue;

            if (typeof(BoundNode).IsAssignableFrom(property.PropertyType) ||
                typeof(IEnumerable<BoundNode>).IsAssignableFrom(property.PropertyType))
                continue;

            var value = property.GetValue(this);
            if (value != null)
                yield return (property.Name, value);
        }
    }

    public void WriteTo(TextWriter writer)
    {
        Print(writer, this);
    }

    private static void Print(TextWriter textWriter, BoundNode node, string indent = "", bool isLast = false)
    {
        var isToConsole = textWriter == Console.Out;
        var marker = isLast ? "\u2514\u2500\u2500" : "\u251c\u2500\u2500";
        textWriter.Write(indent);
        textWriter.Write(marker);

        if (isToConsole)
            Console.ForegroundColor = GetColor(node);
        var text = GetText(node);
        textWriter.Write(text);
         var isFirstProperty = true;
        foreach (var property in node.GetProperites())
        {
            if(isFirstProperty)
                isFirstProperty = false;
            else
            {
                if (isToConsole)
                    Console.ForegroundColor =ConsoleColor.DarkGray;
                textWriter.Write(",");
            }
            textWriter.Write(" ");
            if (isToConsole)
                Console.ForegroundColor =ConsoleColor.Yellow;
            textWriter.Write(property.name);
            
            if (isToConsole)
                Console.ForegroundColor =ConsoleColor.DarkGray;
            
            textWriter.Write(" = ");
            
            if (isToConsole)
                Console.ForegroundColor =ConsoleColor.DarkYellow;
            textWriter.Write(property.value);
        }
        if (isToConsole)
            Console.ResetColor();

        textWriter.WriteLine();
        indent += isLast ? "   " : "\u2502  ";
        var lastChild = node.GetChildren().LastOrDefault();
        foreach (var child in node.GetChildren())
        {
            Print(textWriter, child, indent, child == lastChild);
        }
    }

    private static void WriteProperties(TextWriter textWriter, BoundNode node)
    {
        foreach (var property in node.GetProperites())
        {
            //Console.Write();
        }
    }

    private static void WriteMode(TextWriter textWriter, BoundNode node)
    {
        Console.ForegroundColor = GetColor(node);
        var text = GetText(node);
        textWriter.Write(text);
        Console.ResetColor();
    }

    private static string GetText(BoundNode node)
    {
        if (node is BoundBinaryExpression b)
            return $"{b.Op.Kind.ToString()}Expression";
        if (node is BoundUnaryExpression u)
            return $"{u.Op.Kind.ToString()}Expression";

        return node.Kind.ToString();
    }

    private static ConsoleColor GetColor(BoundNode node)
    {
        if (node is BoundExpression)
            return ConsoleColor.DarkCyan;
        if (node is BoundStatement)
            return ConsoleColor.Magenta;
        return ConsoleColor.Green;
    }


    public override string ToString()
    {
        using var writer = new StringWriter();
        WriteTo(writer);
        return writer.ToString();
    }
}