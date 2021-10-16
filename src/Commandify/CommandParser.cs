using System.Globalization;
using Commandify.Abstractions;
using Commandify.Abstractions.Configuration;
using Commandify.Abstractions.Results;

namespace Commandify;

public class CommandParser : ICommandParser
{
    private static readonly CommandNameRetrieverDelegate CommandIdentifierRetrieverDelegate = (name, _) => name;

    private readonly ILocalizedCommandParser _localizedCommandParser;

    public CommandParser(
        ICommandParserConfiguration commandParserConfiguration,
        IEnumerable<IArgumentTypeResolver>? argumentTypeResolvers = null)
    {
        _localizedCommandParser = new LocalizedCommandParser(
            commandParserConfiguration,
            CommandIdentifierRetrieverDelegate,
            argumentTypeResolvers ?? new IArgumentTypeResolver[] { });
    }

    public ICommandParseResult Parse(ICommand command, string commandText)
    {
        return _localizedCommandParser.Parse(command, CultureInfo.CurrentCulture, commandText);
    }

    public ICommandParseResult<TArguments> Parse<TArguments>(ICommand<TArguments> command, string commandText)
        where TArguments : IArguments, new()
    {
        return _localizedCommandParser.Parse(command, CultureInfo.CurrentCulture, commandText);
    }
}