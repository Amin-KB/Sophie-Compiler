namespace Compiler.CodeAnalysis.Syntax;

public sealed class ForStatementSyntax:StatementSyntax
{
    public override SyntaxKind SyntaxKind => SyntaxKind.ForStatement;
    public SyntaxToken Keyword { get; }
    public SyntaxToken Identifier { get; }
    public SyntaxToken EqualToken { get; }
    public ExpressionSyntax LowerBound { get; }
    public SyntaxToken ToKeyword { get; }
    public ExpressionSyntax UpperBound { get; }
    public StatementSyntax Body { get; }

    public ForStatementSyntax(SyntaxToken keyword,SyntaxToken identifier, SyntaxToken equalToken,
        ExpressionSyntax lowerBound, SyntaxToken toKeyword,ExpressionSyntax upperBound,StatementSyntax body)
    {
        Keyword = keyword;
        Identifier = identifier;
        EqualToken = equalToken;
        LowerBound = lowerBound;
        ToKeyword = toKeyword;
        UpperBound = upperBound;
        Body = body;
    }
}