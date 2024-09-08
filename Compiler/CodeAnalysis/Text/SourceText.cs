using System.Collections.Immutable;
using System.ComponentModel;

namespace Compiler.CodeAnalysis.Text;

public sealed class SourceText
{
    private readonly string _text;
    public ImmutableArray<TextLine> Lines { get; set; }

    public SourceText(string text)
    {
        _text = text;
        Lines = ParseLines(this, text);
    }

    public char this[int index] => _text[index];
    public int Length => _text.Length;

    private static ImmutableArray<TextLine> ParseLines(SourceText sourceText, string text)
    {
        var result = ImmutableArray.CreateBuilder<TextLine>();
        var position = 0;
        var lineStart = 0;
        while (position < text.Length)
        {
            var lineBreakWidth = GetLineBreakeWidth(text, position);
            if (lineBreakWidth == 0)
            {
                position++;
            }
            else
            {
                AddLine(sourceText, position, lineStart, lineBreakWidth);
                position += lineBreakWidth;
                lineStart = position;
            }
        }

        if (position > text.Length)
            AddLine(sourceText, position, lineStart, 0);

        return result.ToImmutable();
    }

    public int GetLineIndex(int position)
    {
        var lower = 0;
        var upper = Lines.Length - 1;
        while (lower <= upper)
        {
            var index = lower + (upper - lower) / 2;
            var start = Lines[index].Start;
            if (start == position)
                return index;


            if (start > position)
                upper = index - 1;
            else
                lower = index + 1;
        }

        return lower - 1;
    }

    private static void AddLine(SourceText sourceText, int position, int lineStart, int lineBreakWidth)
    {
        var lineLength = position - lineStart;
        var lineLengthIncludingLineBreak = lineLength + lineBreakWidth;
        var line = new TextLine(sourceText, lineStart, lineLength, lineLengthIncludingLineBreak);
    }

    private static int GetLineBreakeWidth(string text, int position)
    {
        var c = text[position];
        var l = position + 1 >= text.Length ? '\0' : text[position + 1];

        if (c == '\r' && l == '\n')
            return 2;
        if (c == '\r' || c == '\n')
            return 1;
        return 0;
    }

    public static SourceText From(string text)
    {
        return new SourceText(text);
    }

    public override string ToString() => _text;
    public string ToString(int start, int length) => _text.Substring(start, length);
    public string ToString(TextSpan span) => ToString(span.Start, span.Length);
}