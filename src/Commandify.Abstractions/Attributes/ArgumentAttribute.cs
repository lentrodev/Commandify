namespace Commandify.Abstractions.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ArgumentAttribute : Attribute
{
    public int Position { get; set; }
}