using System.Collections.Immutable;
using System.Reflection;

namespace Compiler.CodeAnalysis.Symbols;

internal static class BuiltinFunctions
{
    public static readonly FunctionSymbol Print = new FunctionSymbol("print", ImmutableArray.Create(new ParameterSymbol("text", TypeSymbol.String)), TypeSymbol.Void);
    public static readonly FunctionSymbol Input = new FunctionSymbol("input", ImmutableArray<ParameterSymbol>.Empty, TypeSymbol.String);
    public static readonly FunctionSymbol Rnd = new FunctionSymbol("rnd",  ImmutableArray.Create(new ParameterSymbol("max", TypeSymbol.Int)), TypeSymbol.Int);
    public static readonly FunctionSymbol IntConvert = new FunctionSymbol("intConvert",  ImmutableArray.Create(new ParameterSymbol("text", TypeSymbol.Int)), TypeSymbol.Int);

    internal static IEnumerable<FunctionSymbol> GetAll()
    => typeof(BuiltinFunctions).GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(f=>f.FieldType==typeof(FunctionSymbol))
        .Select(f => (FunctionSymbol)f.GetValue(null));
}
