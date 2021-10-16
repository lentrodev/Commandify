namespace Commandify.Abstractions.Results;

public interface ICommandParseResult
{
    bool Success { get; }

    ICommand Command { get; }

    string ResultText { get; }
}

public interface ICommandParseResult<TArguments> : ICommandParseResult
    where TArguments : IArguments, new()
{
    TArguments Arguments { get; }
}