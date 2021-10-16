using Commandify.Abstractions;
using Commandify.Abstractions.Results;

namespace Commandify.Results;

public class CommandParseResult : ICommandParseResult
{
    public CommandParseResult(bool success, ICommand command, string resultText)
    {
        Success = success;
        Command = command;
        ResultText = resultText;
    }

    public bool Success { get; }
    public ICommand Command { get; }
    public string ResultText { get; }
}

public class CommandParseResult<TArguments> : CommandParseResult, ICommandParseResult<TArguments>
    where TArguments : IArguments, new()
{
    public CommandParseResult(bool success, ICommand<TArguments> command, string resultText, TArguments arguments) :
        base(success, command, resultText)
    {
        Arguments = arguments;
    }

    public TArguments Arguments { get; }
}