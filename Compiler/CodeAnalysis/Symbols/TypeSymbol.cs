namespace Compiler.CodeAnalysis.Symbols;

public sealed class TypeSymbol : Symbol
{
    public override SymbolKind SymbolKind => SymbolKind.Variable;
    internal TypeSymbol(string name) : base(name) { }
}
