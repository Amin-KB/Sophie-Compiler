using System.Collections.Immutable;

namespace Compiler.CodeAnalysis.Symbols;

public sealed class FunctionSymbol : Symbol
{
    public override SymbolKind SymbolKind => SymbolKind.Function;

    public ImmutableArray<ParameterSymbol> Parameter { get; }
    public TypeSymbol Type { get; }

    public FunctionSymbol(string name, ImmutableArray<ParameterSymbol> parameter, TypeSymbol type) : base(name)
    {
        Parameter = parameter;
        Type = type;
    }
}
