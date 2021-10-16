using Commandify.Abstractions;
using Commandify.Abstractions.Attributes;

public class PingArguments : IArguments
{
    [Argument] public string Url { get; set; }
}