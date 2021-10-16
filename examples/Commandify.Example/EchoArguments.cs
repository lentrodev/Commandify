using Commandify.Abstractions;
using Commandify.Abstractions.Attributes;

public class EchoArguments : IArguments
{
    [Argument] public string Echo { get; set; }
}