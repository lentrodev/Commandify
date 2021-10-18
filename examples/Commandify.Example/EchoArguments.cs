using Commandify.Abstractions;
using Commandify.Abstractions.Attributes;

namespace Commandify.Example;

public class EchoArguments : IArguments
{
    [Argument] public string Echo { get; set; }
}