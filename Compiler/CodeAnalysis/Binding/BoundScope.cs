using System.Collections.Immutable;

namespace Compiler.CodeAnalysis.Binding;

internal sealed class BoundScope
{
    private Dictionary<string, VariableSymbol> _variables = new Dictionary<string, VariableSymbol>();
    public BoundScope Parent { get; }

    public BoundScope(BoundScope parent)
    {
        Parent = parent;
    }

    public bool TryDeclareVariable(VariableSymbol variable)
    {
        if (_variables.ContainsKey(variable.Name))
            return false;
        _variables.Add(variable.Name, variable);
        return true;
    }

    public bool TryLookupVariable(string name, out VariableSymbol variable)
    {
        if (_variables.TryGetValue(name, out variable))
            return true;
        if (Parent == null)
            return false;
        return Parent.TryLookupVariable(name, out variable);
    }

    public ImmutableArray<VariableSymbol> GetDeclaredVariables()
    {
        return _variables.Values.ToImmutableArray();
    }
}