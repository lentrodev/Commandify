using Commandify.Abstractions.Results;

namespace Commandify.Abstractions;

public interface ICommandParser
{
    ICommandParseResult Parse(ICommand command, string commandText);

    ICommandParseResult<TArguments> Parse<TArguments>(ICommand<TArguments> command, string commandText)
        where TArguments : IArguments, new();
}