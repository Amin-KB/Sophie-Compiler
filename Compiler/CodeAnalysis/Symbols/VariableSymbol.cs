namespace Compiler.CodeAnalysis.Symbols;
public sealed class VariableSymbol:Symbol
{
    public override SymbolKind SymbolKind => SymbolKind.Variable;
    public bool IsReadOnly { get; }
    public TypeSymbol Type { get; }

    public override string ToString() => Name;
    
    public VariableSymbol(string name, bool isReadOnly, TypeSymbol type):base(name) 
    {
      
        IsReadOnly = isReadOnly;
        Type = type;
    }
}