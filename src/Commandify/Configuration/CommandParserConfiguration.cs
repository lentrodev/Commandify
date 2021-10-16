using Commandify.Abstractions.Configuration;

namespace Commandify.Configuration;

public class CommandParserConfiguration : ICommandParserConfiguration
{
    public static ICommandParserConfiguration Default { get; } = new CommandParserConfiguration
    {
        AdditionalDelimiters = CommandParserStringAdditionalDelimiters.Quote |
                               CommandParserStringAdditionalDelimiters.DoubleQuote,
        StringComparisonOptions = StringComparison.OrdinalIgnoreCase
    };

    public CommandParserStringAdditionalDelimiters AdditionalDelimiters { get; init; }

    public StringComparison StringComparisonOptions { get; init; }
}