using Compiler.CodeAnalysis.Syntax;

namespace Sophie.Tests.SyntaxAnalysis.Syntax;

internal sealed class AssertingEnumerator : IDisposable
{
    private IEnumerator<SyntaxNode> _enumerator;
    private bool _hasErrors;

    public AssertingEnumerator(SyntaxNode node)
    {
        _enumerator = FlattenTree(node).GetEnumerator();
    }

    private bool HasFailed()
    {
        _hasErrors = true;
        return false;
    }
    public static IEnumerable<SyntaxNode> FlattenTree(SyntaxNode node)
    {
        var stack = new Stack<SyntaxNode>();
        stack.Push(node);

        while (stack.Count > 0)
        {
            var n = stack.Pop();
            stack.Pop();
            yield return n;
            foreach (var child in n.GetChildren().Reverse())
                stack.Push(child);
        }
    }

    public void AssertToken(SyntaxKind kind, string text)
    {
        try
        {
            Assert.True(_enumerator.MoveNext());
            var token = Assert.IsType<SyntaxToken>(_enumerator.Current);
            Assert.Equal(kind, token.SyntaxKind);
            Assert.Equal(text, token.Text);
        }
        catch when(HasFailed())
        {
            throw;
        }
    }

    public void AssertNode(SyntaxKind kind)
    {
        try
        {
            Assert.True(_enumerator.MoveNext());
            Assert.IsNotType<SyntaxKind>(_enumerator.Current);
            Assert.Equal(kind, _enumerator.Current.SyntaxKind);
        }
        catch when(HasFailed())
        {
            throw;
        }
    }

    public void Dispose()
    {
        if (!_hasErrors)
            Assert.False(_enumerator.MoveNext());

        _enumerator.Dispose();
    }
}