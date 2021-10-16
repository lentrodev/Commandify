using Commandify.Abstractions.Configuration;

namespace Commandify.Abstractions.Builders;

public interface ICommandParserBuilder
{
    ICommandParserBuilder UseDefaultConfiguration();

    ICommandParserBuilder UseConfiguration(ICommandParserConfiguration commandParserConfiguration);

    ICommandParserBuilder UseArgumentTypeResolver<TResolver, TArgumentType>()
        where TResolver : IArgumentTypeResolver<TArgumentType>, new();

    ICommandParserBuilder UseArgumentTypeResolver<TResolver, TArgumentType>(TResolver resolver)
        where TResolver : IArgumentTypeResolver<TArgumentType>;

    ICommandParserBuilder UseArgumentTypeResolver<TArgumentType>(IArgumentTypeResolver<TArgumentType> resolver);

    ICommandParser Build();

    ILocalizedCommandParser BuildLocalized(CommandNameRetrieverDelegate commandNameRetriever);
}