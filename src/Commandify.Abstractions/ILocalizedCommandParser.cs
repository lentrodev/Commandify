using System.Globalization;
using Commandify.Abstractions.Results;

namespace Commandify.Abstractions;

public interface ILocalizedCommandParser
{
    CommandNameRetrieverDelegate CommandNameRetriever { get; }

    ICommandParseResult Parse(ICommand command, CultureInfo cultureInfo, string commandText);

    ICommandParseResult<TArguments> Parse<TArguments>(ICommand<TArguments> command, CultureInfo cultureInfo,
        string commandText) where TArguments : IArguments, new();
}