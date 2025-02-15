﻿namespace Compiler.CodeAnalysis.Symbols;

public abstract class Symbol
{
    private protected Symbol(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public abstract SymbolKind SymbolKind { get; }
}
