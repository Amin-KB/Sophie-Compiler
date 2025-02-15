namespace Compiler.CodeAnalysis.Symbols;
public sealed class VariableSymbol:Symbol
{
    public override SymbolKind SymbolKind => SymbolKind.Variable;
    public bool IsReadOnly { get; }
    public Type Type { get; }

    public override string ToString() => Name;
    
    public VariableSymbol(string name, bool isReadOnly, Type type):base(name) 
    {
      
        IsReadOnly = isReadOnly;
        Type = type;
    }
}