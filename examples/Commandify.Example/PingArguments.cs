using Commandify.Abstractions;
using Commandify.Abstractions.Attributes;

namespace Commandify.Example;

public class PingArguments : IArguments
{
    [Argument] public string Url { get; set; }
}