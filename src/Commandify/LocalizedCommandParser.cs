using System.Globalization;
using Commandify.Abstractions;
using Commandify.Abstractions.Configuration;
using Commandify.Abstractions.Results;
using Commandify.Results;

namespace Commandify;

public class LocalizedCommandParser : ILocalizedCommandParser
{
    private readonly IEnumerable<IArgumentTypeResolver> _argumentTypeResolvers;
    private readonly ICommandParserConfiguration _commandParserConfiguration;

    private readonly string _splitRegex;
    private bool _includeQuote, _includeDoubleQuote;

    public LocalizedCommandParser(
        ICommandParserConfiguration commandParserConfiguration,
        CommandNameRetrieverDelegate commandNameRetriever,
        IEnumerable<IArgumentTypeResolver>? argumentTypeResolvers = null)
    {
        _commandParserConfiguration = commandParserConfiguration;
        _argumentTypeResolvers = argumentTypeResolvers ?? new IArgumentTypeResolver[] { };
        CommandNameRetriever = commandNameRetriever;

        _splitRegex = BuildSplitRegex();
    }

    public CommandNameRetrieverDelegate CommandNameRetriever { get; }

    public ICommandParseResult Parse(ICommand command, CultureInfo cultureInfo, string commandText)
    {
        var stringSegmentCollection =
            new CommandDataSegmentCollection(commandText, _splitRegex, _includeQuote, _includeDoubleQuote);

        var commandName = CommandNameRetriever(command.Id, cultureInfo);

        var matches = true;

        foreach (var commandNameSegment in commandName.Split(" "))
        {
            if (!stringSegmentCollection.SegmentsAvailable)
            {
                return new CommandParseResult(false, command, stringSegmentCollection.ToString());
            }
            
            if (string.Compare(commandNameSegment, stringSegmentCollection.Take(),
                    _commandParserConfiguration.StringComparisonOptions) != 0)
            {
                matches = false;
                break;
            }
        }

        return new CommandParseResult(matches, command, stringSegmentCollection.ToString());
    }

    public ICommandParseResult<TArguments> Parse<TArguments>(ICommand<TArguments> command, CultureInfo cultureInfo,
        string commandText) where TArguments : IArguments, new()
    {
        var stringSegmentCollection =
            new CommandDataSegmentCollection(commandText, _splitRegex, _includeQuote, _includeDoubleQuote);

        var commandName = CommandNameRetriever(command.Id, cultureInfo);

        var matches = true;

        foreach (var commandNameSegment in commandName.Split(" "))
        {
            if (!stringSegmentCollection.SegmentsAvailable)
            {
                return new CommandParseResult<TArguments>(false, command, stringSegmentCollection.ToString(), default);
            }
            if (string.Compare(commandNameSegment, stringSegmentCollection.Take(),
                    _commandParserConfiguration.StringComparisonOptions) != 0)
            {
                matches = false;
                break;
            }
        }

        var arguments = new TArguments();

        var argumentDescriptors = ArgumentsExtensions.ValidateArguments<TArguments>()
            //Order by position later
            ;


        var allPropertiesSet = true;

        string argumentSegment;

        foreach (var argumentDescriptor in argumentDescriptors)
            if (stringSegmentCollection.SegmentsAvailable)
            {
                argumentSegment = stringSegmentCollection.Take();

                object argumentValue = null;

                var argumentType = argumentDescriptor.PropertyInfo.PropertyType;

                if (Type.GetTypeCode(argumentType) is { } typeCode and not TypeCode.Empty and not TypeCode.Object)
                    argumentValue = Convert.ChangeType(argumentSegment, typeCode);
                else
                    foreach (var resolver in _argumentTypeResolvers)
                        if (resolver.GetType() is { } type
                            && type.GetGenericArguments() is { Length: > 0 } genericArguments
                            && genericArguments.First() == argumentType)
                            argumentValue = resolver.GetType()!.GetMethod("Resolve")!
                                .Invoke(resolver, new object?[] { argumentSegment });

                if (argumentValue is null) throw new ArgumentException("Cannot parse argument");

                argumentDescriptor.PropertyInfo.SetValue(arguments, argumentValue);
            }
            else
            {
                allPropertiesSet = false;
                break;
            }


        return new CommandParseResult<TArguments>(matches && allPropertiesSet, command,
            stringSegmentCollection.ToString(), arguments);
    }

    private string BuildSplitRegex()
    {
        var additionalDelimiters = _commandParserConfiguration.AdditionalDelimiters;

        bool includeQuote = _includeQuote = additionalDelimiters.HasFlag(CommandParserStringAdditionalDelimiters.Quote),
            includeDoubleQuote = _includeDoubleQuote =
                additionalDelimiters.HasFlag(CommandParserStringAdditionalDelimiters.DoubleQuote);

        string quotePart = includeQuote ? "'" : "",
            doubleQuotePart = includeQuote ? "\"" : "";

        var regex = $"[^\\s{doubleQuotePart}{quotePart}]+";

        if (includeDoubleQuote) regex += "|\"([^\"]+)\"";

        if (includeQuote) regex += "|\'([^\']+)\'";

        return regex;
    }
}