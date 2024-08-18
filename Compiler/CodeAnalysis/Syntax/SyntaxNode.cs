using System.Reflection;

namespace Compiler.CodeAnalysis.Syntax;

public abstract class SyntaxNode
{
    public abstract SyntaxKind SyntaxKind { get; }
    public IEnumerable<SyntaxNode> GetChildren()
    {
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            if (typeof(SyntaxNode).IsAssignableFrom(property.PropertyType))
            {
                var child = (SyntaxNode)property.GetValue(this);
                yield return child;
            }
            else if (typeof(IEnumerable<SyntaxNode>).IsAssignableFrom(property.PropertyType))
            {
                var children = (IEnumerable<SyntaxNode>)property.GetValue(this);
                foreach (var child in children)
                    yield return child;
            }
        }
    }
    public void WriteTo(TextWriter writer)
    {
        Print(writer, this);
    }
    private static void Print(TextWriter textWriter, SyntaxNode node, string indent = "", bool isLast = false)
    {
        var marker = isLast ? "\u2514\u2500\u2500" : "\u251c\u2500\u2500";
        textWriter.Write(indent);
        textWriter.Write(marker);
        textWriter.Write(node.SyntaxKind);
        if (node is SyntaxToken t && t.Value != null)
        {
            textWriter.Write(" ");
            textWriter.Write(t.Value);
        }

        textWriter.WriteLine();
        indent += isLast ? "   " : "\u2502  ";
        var lastChild = node.GetChildren().LastOrDefault();
        foreach (var child in node.GetChildren())
        {
            Print(textWriter, child, indent, child == lastChild);
        }
    }
}