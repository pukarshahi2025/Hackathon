using System;

[AttributeUsage(AttributeTargets.Property)]
public class FixedLengthFieldAttribute : Attribute
{
    public int Length { get; }
    public int Start { get; }

    public FixedLengthFieldAttribute(int length, int start)
    {
        Length = length;
        Start = start;
    }
}
