namespace Compiler.CodeAnalysis;

public struct TextSpan
{
    public TextSpan(int start, int length)
    {
        Start = start;
        Length = length;
    }

    public int Length { get;  }

    public int Start { get;  }
    public int End => Start + Length;
}