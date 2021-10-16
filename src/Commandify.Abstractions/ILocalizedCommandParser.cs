using System.Globalization;
using Commandify.Abstractions.Results;

namespace Commandify.Abstractions;

public interface ILocalizedCommandParser
{
    CommandNameRetrieverDelegate CommandNameRetriever { get; }

    ICommandParseResult Parse(ICommand command, CultureInfo cultureInfo, string commandText);

    /// <summary>
    ///     Parses command with arguments.
    /// </summary>
    /// <param name="command">Command to be parsed.</param>
    /// <param name="cultureInfo">Language provided.</param>
    /// <param name="commandText"></param>
    /// <typeparam name="TArguments"></typeparam>
    /// <returns></returns>
    ICommandParseResult<TArguments> Parse<TArguments>(ICommand<TArguments> command, CultureInfo cultureInfo,
        string commandText) where TArguments : IArguments, new();
}