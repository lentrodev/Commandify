using Commandify.Processing.Abstractions;

namespace Commandify.Example;

public class CommandContext : ICommandContext
{
    public string Text { get; set; }
}