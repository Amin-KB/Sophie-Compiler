﻿using Compiler.CodeAnalysis;
using Compiler.CodeAnalysis.Symbols;
using Compiler.CodeAnalysis.Syntax;

namespace Sophie.Tests.SyntaxAnalysis;

public class EvaluatorTest
{
    [Theory]
    [InlineData("1", 1)]
    [InlineData("+1", 1)]
    [InlineData("-1", -1)]
    [InlineData("~1", -2)]
    [InlineData("14 + 12", 26)]
    [InlineData("12 - 3", 9)]
    [InlineData("4 * 2", 8)]
    [InlineData("9 / 3", 3)]
    [InlineData("(10)", 10)]
    [InlineData("12 == 3", false)]
    [InlineData("3 == 3", true)]
    [InlineData("12 != 3", true)]
    [InlineData("12 > 3", true)]
    [InlineData("12 < 3", false)]
    [InlineData("12 >= 3", true)]
    [InlineData("12 >= 12", true)]
    [InlineData("12 <= 3", false)]
    [InlineData("3 > 1", true)]
    [InlineData("3 <= 3", true)]
    [InlineData("3 != 3", false)]
    
    [InlineData("1 | 2", 3)]
    [InlineData("1 | 0", 1)]
    [InlineData("1 & 3", 1)]
    [InlineData("1 & 0", 0)]
    [InlineData("1^0", 1)]
    [InlineData("0 ^ 1", 1)]
    [InlineData("1 ^ 3", 2)]
    
    [InlineData("false == false", true)]
    [InlineData("true == false", false)]
    [InlineData("false != false", false)]
    [InlineData("true != false", true)]
    [InlineData("true", true)]
    [InlineData("false", false)]
    [InlineData("!true", false)]
    [InlineData("!false", true)]
    [InlineData("{var a = 0 (a = 10) * a }", 100)]
    [InlineData("{ var a = 0 if a == 0 a = 10  a }", 10)]
    [InlineData("{ var a = 0 if a == 4 a = 10  a }", 0)]
    [InlineData("{ var a = 0 if a == 0 a = 10 else a = 5  a }", 10)]
    [InlineData("{ var a = 0 if a == 4 a = 10 else a = 5  a }", 5)]
    [InlineData("{ var i = 10 var result =0  while i > 0 {result = result + i i = i - 1 } result }", 55)]
    [InlineData("{ var result = 0 for i = 1 to 10 {result = result + i } result }", 55)]
    [InlineData("{ var a = 10 for i = 1 to (a = a - 1 ){} a }", 9)]
    public void Evaluator_Computes_CorrectValues(string text, object expectedValue)
    {
        AssertValue(text, expectedValue);
    }


    [Fact]
    public void Evaluator_VariableDeclaration_Reports_Redeclaration()
    {
        var text = @"
               {
                 var x = 10
                 var y = 100
                     {
                      var x = 10
                      }
                  var [x] = 5
                }";


        var diagnostics = @" 
         Variable 'x' is already declared.
           ";
        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_Name_Reports_Undefined()
    {
        var text = @"[x] * 10";


        var diagnostics = @" 
         Variable name 'x' does not exist.
           ";
        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_Assignment_Reports_Undefined()
    {
        var text = @"
                {
                  let x = 40
                  x [=] 10
                }";


        var diagnostics = @" 
         Variable 'x' is read-only and cannot be assigned to.
           ";
        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_Assignment_Reports_CannotConvert()
    {
        var text = @"
                {
                  var x = 40
                  x = [true]
                }";


        var diagnostics = @" 
         cannot convert type  'bool' to 'int'.
           ";
        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_IFStatement_Reports_CannotConvert()
    {
        var text = @"
                {
                  var x = 0
                  if [10]
                     x= 10
                }";

        var diagnostics = @" 
         cannot convert type  'int' to 'bool'.
           ";
        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_WhileStatement_Reports_CannotConvert()
    {
        var text = @"
                {
                  var x = 0
                  while [10]
                     x= 10
                }";

        var diagnostics = @" 
         cannot convert type  'int' to 'bool'.
           ";
        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_ForStatement_Reports_CannotConvert()
    {
        var text = @"
                {
                  var result = 0
                  for i = [false] to 10
                     result =result + i
                }";

        var diagnostics = @" 
         cannot convert type  'bool' to 'int'.
           ";
        AssertDiagnostics(text, diagnostics);
    }



    [Fact]
    public void Evaluator_BlockStatement_Reports_NotInfinateLoop()
    {
        var text = @"
                    {
                    [)][]
                   ";


        var diagnostics = @" 
        ERROR: Unexpected token <CloseParenthesisToken> , expected <IdentifierToken>.
        ERROR: Unexpected token <EndOfFileToken> , expected <CloseBraceToken>.
           ";
        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_NameExpression_Reports_NoErrorForInsertedToken()
    {
        var text = @"[]";
        var diagnostics = @"
      ERROR: Unexpected token <EndOfFileToken> , expected <IdentifierToken>.
       ";
        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_Binary_Reports_Undefined()
    {
        var text = @"10 [+] true";


        var diagnostics = @" 
        Binary operator '+' is not defined for type int and bool.
           ";
        AssertDiagnostics(text, diagnostics);
    }

    private static void AssertValue(string text, object expectedValue)
    {
        var syntaxTree = SyntaxTree.Parse(text);
        var compilation = new Compilation(syntaxTree);
        var variables = new Dictionary<VariableSymbol, object>();
        var result = compilation.Evaluate(variables);
        Assert.Empty(result.Diagnostics);
        Assert.Equal(expectedValue, result.Value);
    }

    private void AssertDiagnostics(string text, string diagnosticText)
    {
        var annotatedText = AnnotatedText.Parse(text);
        var syntaxTree = SyntaxTree.Parse(annotatedText.Text);
        var compilation = new Compilation(syntaxTree);
        var result = compilation.Evaluate(new Dictionary<VariableSymbol, object>());
        var expectedDiagnostics = AnnotatedText.UnindentLines(diagnosticText);
        if (annotatedText.Spans.Length != expectedDiagnostics.Length)
            throw new Exception("ERROR: Must mark as many spans as there are expected");
        Assert.Equal(expectedDiagnostics.Length, result.Diagnostics.Length);
        for (var i = 0; i < expectedDiagnostics.Length; i++)
        {
            var expectedMessage = expectedDiagnostics[i];
            var actualMessage = result.Diagnostics[i].Message;
            Assert.Equal(expectedMessage, actualMessage);
            var expectedSpan = annotatedText.Spans[i];
            var actualSpan = result.Diagnostics[i].TextSpan;
            Assert.Equal(expectedSpan, actualSpan);
        }
    }
}