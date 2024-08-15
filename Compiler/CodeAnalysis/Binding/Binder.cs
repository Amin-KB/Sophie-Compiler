﻿using System.Security.Cryptography.X509Certificates;
using Compiler.CodeAnalysis.Syntax;

namespace Compiler.CodeAnalysis.Binding;

internal sealed class Binder
{
    private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
    public DiagnosticBag Diagnostics => _diagnostics;

    public Binder(Dictionary<VariableSymbol, object> variables)
    {
        _variables = variables;
    }

    private readonly Dictionary<VariableSymbol, object> _variables;

    public BoundExpression BindExpression(ExpressionSyntax syntax)
    {
        switch (syntax.SyntaxKind)
        {
            case SyntaxKind.ParenthesizedExpression:
                return BindParenthesizedExpression((ParenthesizedExpressionSyntax)syntax);
            case SyntaxKind.LiteralExpression:
                return BindLiteralExpression((LiteralExpressionSyntax)syntax);
            case SyntaxKind.NameExpression:
                return BindNameExpression((NameExpressionSyntax)syntax);
            case SyntaxKind.AssignmentEsxpression:
                return BindAssignmentExpression((AssignmentExpressionSyntax)syntax);
            case SyntaxKind.UnaryExpression:
                return BindUnaryExpression((UnaryExpressionSyntax)syntax);
            case SyntaxKind.BinaryExpression:
                return BindBinaryExpression((BinaryExpressionSyntax)syntax);
            default:
                throw new Exception($"Unexpected syntax {syntax.SyntaxKind}");
        }
    }

    private BoundExpression BindParenthesizedExpression(ParenthesizedExpressionSyntax syntax)
    {
        return BindExpression(syntax.Expression);
    }

    private BoundExpression BindNameExpression(NameExpressionSyntax syntax)
    {
        var name = syntax.IdentifierToken.Text;
        var variable = _variables.Keys.FirstOrDefault(v => v.Name == name);
        if (variable == null)
        {
            _diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);
        }


        return new BoundVariableExpression(variable);
    }

    private BoundExpression BindAssignmentExpression(AssignmentExpressionSyntax syntax)
    {
        var name = syntax.IdentifierToken.Text;
        var boundExpression = BindExpression(syntax.Expression);
        var existingVariable = _variables.Keys.FirstOrDefault(v => v.Name == name);
        if (existingVariable != null)
        {
            _variables.Remove(existingVariable);
        }

        var variable = new VariableSymbol(name, boundExpression.Type);
        _variables[variable] = null;


        return new BoundAssignmentExpression(variable, boundExpression);
    }

    private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
    {
        var value = syntax.Value ?? 0;
        return new BoundLiteralExpression(value);
    }

    private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
    {
        var boundOperand = BindExpression(syntax.Operand);
        var boundOperator = BoundUnaryOperator.Bind(syntax.OperatorToken.SyntaxKind, boundOperand.Type);
        if (boundOperator == null)
        {
            _diagnostics.ReportUndefinedUnaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text,
                boundOperand.Type);
            return boundOperand;
        }

        return new BoundUnaryExpression(boundOperator, boundOperand);
    }

    private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
    {
        var boundLeft = BindExpression(syntax.Left);
        var boundRight = BindExpression(syntax.Right);
        var boundOperator =
            BoundBinaryOperator.Bind(syntax.OperatorToken.SyntaxKind, boundLeft.Type, boundRight.Type);
        if (boundOperator == null)
        {
            _diagnostics.ReportUndefinedBinaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text,
                boundLeft.Type, boundRight.Type);
            return boundLeft;
        }

        return new BoundBinaryExpression(boundLeft, boundOperator, boundRight);
    }
}