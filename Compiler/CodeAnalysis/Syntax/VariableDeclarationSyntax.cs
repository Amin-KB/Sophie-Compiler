namespace Compiler.CodeAnalysis.Syntax;

public sealed class VariableDeclarationSyntax:StatementSyntax
{
    public override SyntaxKind SyntaxKind => SyntaxKind.VariableDeclaration;
    public SyntaxToken Keyword { get; }
    public SyntaxToken Identifier { get; }
    public TypeClauseSyntax TypeClause { get; }
    public SyntaxToken EqualsToken { get; }
    public ExpressionSyntax Initializer { get; }

    public VariableDeclarationSyntax(SyntaxToken keyword, SyntaxToken identifier,TypeClauseSyntax typeClause, SyntaxToken equalsToken,
                                    ExpressionSyntax initializer)
    {
        Keyword = keyword;
        Identifier = identifier;
        TypeClause = typeClause;
        EqualsToken = equalsToken;
        Initializer = initializer;
    }

}