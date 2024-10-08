namespace Compiler.CodeAnalysis.Text;

public sealed class TextLine
{
    public SourceText Text { get; }
    public int Start { get; }
    public int Length { get; }

    public int End => Start + Length;
    public int LengthIncludingLineBreak { get; }
    public TextSpan Span => new TextSpan(Start, Length);
    public TextSpan SpanIncludingLineBreak => new TextSpan(Start, LengthIncludingLineBreak);

    public TextLine(SourceText text, int start, int length, int lengthIncludingLineBreak)
    {
        Text = text;
        Start = start;
        Length = length;
        LengthIncludingLineBreak = lengthIncludingLineBreak;
    }

    public override string ToString() => Text.ToString(Span);
}