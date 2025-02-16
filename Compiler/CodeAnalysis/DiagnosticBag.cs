using System.Collections;
using System.Xml.Linq;
using Compiler.CodeAnalysis.Symbols;
using Compiler.CodeAnalysis.Syntax;
using Compiler.CodeAnalysis.Text;

namespace Compiler.CodeAnalysis;

internal sealed class DiagnosticBag : IEnumerable<Diagnostic>
{
    private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

    private void Report(TextSpan span, string message)
    {
        var diagnostic = new Diagnostic(span, message);
        _diagnostics.Add(diagnostic);
    }

    public IEnumerator<Diagnostic> GetEnumerator()
        => _diagnostics.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public void ReportInvalidNumber(TextSpan textSpan, string text, TypeSymbol type)
    {
        var message = $"The Number {text} is not valid {type}";
        Report(textSpan,text);
    }

    public void ReportBadCharacter(int position, char character)
    {
        var message = $"ERROR: bad character input: '{character}";
        Report(new TextSpan(position,1),message);
    }

    public void AddRange(DiagnosticBag diagnostics)
    {
       _diagnostics.AddRange(diagnostics._diagnostics); 
    }
    public void Concat(DiagnosticBag diagnostics)
    {
        _diagnostics.Concat(diagnostics._diagnostics); 
    }
    public void ReportUnexpectedToken(TextSpan span, SyntaxKind actualKind, SyntaxKind expectedKind)
    {
        var message = $"ERROR: Unexpected token <{actualKind}> , expected <{expectedKind}>.";
        Report(span,message);
    }

    public void ReportUndefinedUnaryOperator(TextSpan span, string operatorText, TypeSymbol operandType)
    {
        var message = $"Unary operator '{operatorText}' is not defined for type {operandType}.";
        Report(span,message);
    }

    public void ReportUndefinedBinaryOperator(TextSpan span, string name, TypeSymbol boundLeftType, TypeSymbol boundRightType)
    {
        var message =
            $"Binary operator '{name}' is not defined for type {boundLeftType} and {boundRightType}.";
        Report(span,message);
    }

    public void ReportUndefinedName(TextSpan span, string name)
    {
        var message =
            $"Variable name '{name}' does not exist.";
        Report(span,message);
    }
    
    public void ReportCannotConvert(TextSpan span, TypeSymbol fromType, TypeSymbol toType)
    {
        var message =
            $"cannot convert type  '{fromType}' to '{toType}'.";
        Report(span,message);
    }

    public void ReportVariableAlreadyDeclared(TextSpan span, string name)
    {
        var message =
            $"Variable '{name}' is already declared.";
        Report(span,message);
    }

    public void ReportCannotAssign(TextSpan span, string name)
    {
        var message =
            $"Variable '{name}' is read-only and cannot be assigned to.";
        Report(span,message);
    }

    public void ReportUnterminatedString(TextSpan span)
    {
        var message =
            $"unterminated string literal.";
        Report(span,message);
    }

    public void ReportUndefinedFunction(TextSpan span, string name)
    {
        var message =
          $"Function '{name}' does not exist.";
        Report(span, message);
    }

    public void ReportWrongArgumentCount(TextSpan span, string name,int expectedCount, int actualCount)
    {
        var message =
          $"Function '{name}' requires {expectedCount} arguments but given {actualCount}";
        Report(span, message);
    }

    public void ReportWrongArgumentType(TextSpan span, string name, string parameterName, TypeSymbol expectedType, TypeSymbol actualType)
    {
        var message =
         $"Parameter '{name}' requires a value of type '{expectedType}' but was given a value of type '{actualType}'";
        Report(span, message);
    }
}