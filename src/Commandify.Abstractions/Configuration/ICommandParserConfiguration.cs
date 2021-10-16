namespace Commandify.Abstractions.Configuration;

public interface ICommandParserConfiguration
{
    CommandParserStringAdditionalDelimiters AdditionalDelimiters { get; }

    StringComparison StringComparisonOptions { get; }
}