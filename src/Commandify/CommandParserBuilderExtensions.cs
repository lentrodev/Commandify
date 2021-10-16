using Commandify.Abstractions.Builders;

namespace Commandify;

public static class CommandParserBuilderExtensions
{
    public static ICommandParserBuilder UseArgumentTypeResolver<T>(this ICommandParserBuilder commandParserBuilder,
        ParseDelegate<T> parseDelegate)
    {
        return commandParserBuilder.UseArgumentTypeResolver(new ArgumentTypeResolver<T>(parseDelegate));
    }

    public static ICommandParserBuilder UseArgumentTypeResolver<T>(this ICommandParserBuilder commandParserBuilder,
        TryParseDelegate<T> tryParseDelegate)
    {
        return commandParserBuilder.UseArgumentTypeResolver(new ArgumentTypeResolver<T>(tryParseDelegate));
    }
}