namespace Compiler.CodeAnalysis.Symbols;

public sealed class ParameterSymbol : VariableSymbol
{
    public override SymbolKind SymbolKind => SymbolKind.Parameter;
    public ParameterSymbol(string name, TypeSymbol type) : base(name, isReadOnly:true,  type) { }
}
