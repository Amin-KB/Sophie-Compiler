﻿using System.Collections.Immutable;

namespace Compiler.CodeAnalysis.Binding;

internal abstract class BoundTreeRewriter
{
    public virtual BoundStatement RewriteStatement(BoundStatement node)
    {
        switch (node.Kind)
        {
            case BoundNodeKind.BlockStatement:
                return RewriteBlockStatement((BoundBlockStatement)node);
            case BoundNodeKind.ExpressionStatement:
                return RewriteExpressionStatement((BoundExpressionStatement)node);
            case BoundNodeKind.IfStatement:
                return RewriteIfStatement((BoundIfStatement)node);
            case BoundNodeKind.WhileStatement:
                return RewriteWhileStatement((BoundWhileStatement)node);
            case BoundNodeKind.ForStatement:
                return RewriteForStatement((BoundForStatement)node);
            case BoundNodeKind.GotoStatement:
                return RewriteGotoStatement((BoundGotoStatement)node);
            case BoundNodeKind.ConditionalGotoStatement:
                return RewriteConditionalGotoStatement((BoundConditionalGotoStatement)node);
            case BoundNodeKind.LabelStatement:
                return RewriteLabelStatement((BoundLabelStatement)node);
            case BoundNodeKind.VariableDeclaration:
                return RewriteVariableDeclaration((BoundVariableDeclaration)node);
            default:
                throw new Exception($"Unexpected node kind: {node.Kind}");
        }
    }

    protected virtual BoundStatement RewriteGotoStatement(BoundGotoStatement node)
    {
        return node;
    }

    protected virtual BoundStatement RewriteConditionalGotoStatement(BoundConditionalGotoStatement node)
    {
        var condition = RewriteExpression(node.Condition);
        if (condition == node.Condition)
            return node;

        return new BoundConditionalGotoStatement(node.Label, condition, node.JumpIfTrue);
    }

    protected virtual BoundStatement RewriteLabelStatement(BoundLabelStatement node)
    {
        return node;
    }

    private BoundStatement RewriteVariableDeclaration(BoundVariableDeclaration node)
    {
        var initializer = RewriteExpression(node.Initializer);
        if (initializer == node.Initializer)
            return node;

        return new BoundVariableDeclaration(node.Variable, initializer);
    }

    protected virtual BoundStatement RewriteForStatement(BoundForStatement node)
    {
        var lowerBound = RewriteExpression(node.LowerBound);
        var upperBound = RewriteExpression(node.UpperBound);
        var body = RewriteStatement(node.Body);
        if (lowerBound == node.LowerBound && upperBound == node.UpperBound && body == node.Body)
            return node;

        return new BoundForStatement(node.Variable, lowerBound, upperBound, body);
    }

    protected virtual BoundStatement RewriteWhileStatement(BoundWhileStatement node)
    {
        var condition = RewriteExpression(node.Condition);
        var body = RewriteStatement(node.Body);
        if (condition == node.Condition && body == node.Body)
            return node;
        return new BoundWhileStatement(condition, body);
    }

    protected virtual BoundStatement RewriteIfStatement(BoundIfStatement node)
    {
        var condition = RewriteExpression(node.Condition);
        var thenStatement = RewriteStatement(node.ThanStatement);
        var elseStatement = node.ElseStatement == null
            ? null
            : RewriteStatement(node.ElseStatement);
        if (condition == node.Condition && thenStatement == node.ThanStatement && elseStatement == node.ElseStatement)
            return node;

        return new BoundIfStatement(condition, thenStatement, elseStatement);
    }

    protected virtual BoundStatement RewriteExpressionStatement(BoundExpressionStatement node)
    {
        var expression = RewriteExpression(node.Expression);
        if (expression != node.Expression)
            return node;

        return new BoundExpressionStatement(expression);
    }

    protected virtual BoundStatement RewriteBlockStatement(BoundBlockStatement node)
    {
        ImmutableArray<BoundStatement>.Builder builder = null;

        for (int i = 0; i < node.Statements.Length; i++)
        {
            var oldStatement = node.Statements[i];
            var newStatement = RewriteStatement(oldStatement);
            if (newStatement != oldStatement)
            {
                if (builder == null)
                {
                    builder = ImmutableArray.CreateBuilder<BoundStatement>(node.Statements.Length);
                    for (int j = 0; j < i; j++)
                        builder.Add(node.Statements[j]);
                }
            }

            if (builder != null)
                builder.Add(newStatement);
        }

        if (builder == null)
            return node;

        return new BoundBlockStatement(builder.MoveToImmutable());
    }


    public virtual BoundExpression RewriteExpression(BoundExpression node)
    {
        switch (node.Kind)
        {

            case BoundNodeKind.ErrorExpression:
                return RewriteErrorExpression((BoundErrorExpression)node);
            case BoundNodeKind.LiteralExpression:
                return RewriteLiteralExpression((BoundLiteralExpression)node);
            case BoundNodeKind.UnaryExpression:
                return RewriteUnaryExpression((BoundUnaryExpression)node);
            case BoundNodeKind.VariableExpression:
                return RewriteVariableExpression((BoundVariableExpression)node);
            case BoundNodeKind.AssignmentExpression:
                return RewriteAssignmentExpression((BoundAssignmentExpression)node);
            case BoundNodeKind.BinaryExpression:
                return RewriteBinaryExpression((BoundBinaryExpression)node);
            case BoundNodeKind.CallExpression:
                return RewriteCallExpression((BoundCallExpression)node);
            case BoundNodeKind.ConversionExpression:
                return RewriteConversionExpression((BoundConversionExpression)node);
            default:
                throw new Exception($"Unexpected node kind: {node.Kind}");
        }
    }

    protected virtual BoundExpression RewriteConversionExpression(BoundConversionExpression node)
    {
        var expression = RewriteExpression(node.Expression);
     
        if (expression == node.Expression )
            return node;

        return new BoundConversionExpression(node.Type, expression);
    }

    protected BoundExpression RewriteCallExpression(BoundCallExpression node)
    {
        ImmutableArray<BoundExpression>.Builder builder = null;

        for (int i = 0; i < node.Arguments.Length; i++)
        {
            var oldArgument = node.Arguments[i];
            var newArgument = RewriteExpression(oldArgument);
            if (newArgument != oldArgument)
            {
                if (builder == null)
                {
                    builder = ImmutableArray.CreateBuilder<BoundExpression>(node.Arguments.Length);
                    for (int j = 0; j < i; j++)
                        builder.Add(node.Arguments[j]);
                }
            }

            if (builder != null)
                builder.Add(newArgument);
        }

        if (builder == null)
            return node;

        return new BoundCallExpression(node.Function,builder.MoveToImmutable());
    }

    protected BoundExpression RewriteErrorExpression(BoundErrorExpression node)
    {
        return node;
    }

    protected virtual BoundExpression RewriteBinaryExpression(BoundBinaryExpression node)
    {
        var left = RewriteExpression(node.Left);
        var right = RewriteExpression(node.Right);
        if (left == node.Left && right == node.Right)
            return node;
        return new BoundBinaryExpression(left, node.Op, right);
    }

    protected virtual BoundExpression RewriteAssignmentExpression(BoundAssignmentExpression node)
    {
        var expression = RewriteExpression(node.Expression);
        if (expression != node.Expression)
            return node;

        return new BoundAssignmentExpression(node.Variable, expression);
    }


    protected virtual BoundExpression RewriteUnaryExpression(BoundUnaryExpression node)
    {
        var operand = RewriteExpression(node.Operand);
        if (operand != node.Operand)
            return node;

        return new BoundUnaryExpression(node.Op, operand);
    }

    protected virtual BoundExpression RewriteVariableExpression(BoundVariableExpression node)
    {
        return node;
    }

    protected virtual BoundExpression RewriteLiteralExpression(BoundLiteralExpression node)
    {
        return node;
    }
}