namespace Compiler.CodeAnalysis.Syntax;

public sealed class BinaryExpressionSyntax:ExpressionSyntax
{
    public override SyntaxKind SyntaxKind => SyntaxKind.BinaryExpression;

  

    /// <summary>
    /// Gets the left side of the expression.
    /// </summary>
    /// <value>
    /// The left side of the expression.
    /// </value>
    public ExpressionSyntax Left { get; }

    /// <summary>
    /// Gets the right expression of the current expression syntax.
    /// </summary>
    /// <remarks>
    /// This property is read-only and returns the right expression of the current expression syntax.
    /// </remarks>
    /// <value>
    /// The right expression.
    /// </value>
    public ExpressionSyntax Right { get; }

    /// <summary>
    /// Gets the syntax token representing the operator.
    /// </summary>
    /// <remarks>
    /// The operator token provides access to the syntax element that represents the operator in the code.
    /// </remarks>
    /// <returns>
    /// The syntax token representing the operator.
    /// </returns>
    public SyntaxToken OperatorToken { get; }

  
    public BinaryExpressionSyntax(ExpressionSyntax left,SyntaxToken operatorToken,ExpressionSyntax right)
    {
        Left = left;
        Right=right;
        OperatorToken = operatorToken;
    }
}