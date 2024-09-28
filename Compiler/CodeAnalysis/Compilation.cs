using System.Collections;
using System.Collections.Immutable;
using Compiler.CodeAnalysis.Binding;
using Compiler.CodeAnalysis.Lowering;
using Compiler.CodeAnalysis.Syntax;

namespace Compiler.CodeAnalysis;

public sealed class Compilation
{
    private BoundGlobalScope _globalScope;
    
    public Compilation(SyntaxTree syntax):this(null, syntax)
    {
        
    }
    private Compilation(Compilation previous,SyntaxTree syntax)
    {
        Previous = previous;
        Syntax = syntax;
    }
    public Compilation Previous { get;  }
    public SyntaxTree Syntax { get;  }

    internal BoundGlobalScope GlobalScope
    {
        get
        {
            if (_globalScope == null)
            {
                var globalScope = Binder.BindGlobalScope(Previous?.GlobalScope,Syntax.Root);
                Interlocked.CompareExchange(ref _globalScope, globalScope, null);
            }
            return _globalScope;
        }
    }


    public Compilation ContinueWith(SyntaxTree syntaxTree)
    {
        return new Compilation(this, syntaxTree);
    }

    public EvaluationResult Evaluate(Dictionary<VariableSymbol,object>variables)
    {
        var diagnostics = Syntax.Diagnostics.Concat(GlobalScope.Diagnostics).ToImmutableArray();
        if (diagnostics.Any())
            return new EvaluationResult(diagnostics, null);
        var statement = GetStatement();
        var evaluator = new Evaluator(statement,variables);
        var value = evaluator.Evaluate();
        return new EvaluationResult(ImmutableArray<Diagnostic>.Empty, value);
    }


    public void EmitTree(TextWriter writer)
    {
        var statement = GetStatement();
        statement.WriteTo(writer);
    }

    private BoundStatement GetStatement()
    {
        var result= GlobalScope.Statement;
       return Lowerer.Lower(result);
    }
}