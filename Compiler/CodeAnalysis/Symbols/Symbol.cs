﻿namespace Compiler.CodeAnalysis.Symbols;

public abstract class Symbol
{
    private protected Symbol(string name)
    {
        Name = name;
    }
    public abstract SymbolKind SymbolKind { get; }
    public string Name { get; }

    public override string ToString() => Name;
  

}
