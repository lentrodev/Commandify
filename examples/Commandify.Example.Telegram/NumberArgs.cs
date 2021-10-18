using Commandify.Abstractions;
using Commandify.Abstractions.Attributes;

namespace Commandify.Example.Telegram;

public class NumberArgs : IArguments
{
    [Argument]
    public int Amount { get; set; }
}