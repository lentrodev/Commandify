using System.Collections.ObjectModel;
using Commandify.Abstractions;
using Commandify.Abstractions.Builders;
using Commandify.Abstractions.Configuration;
using Commandify.Configuration;

namespace Commandify.Builders;

public class CommandParserBuilder : ICommandParserBuilder
{
    private readonly ICollection<IArgumentTypeResolver> _argumentTypeResolvers;
    private ICommandParserConfiguration? _commandParserConfiguration;


    public CommandParserBuilder()
    {
        _commandParserConfiguration = null;
        _argumentTypeResolvers = new Collection<IArgumentTypeResolver>();
    }

    public ICommandParserBuilder UseDefaultConfiguration()
    {
        return UseConfiguration(CommandParserConfiguration.Default);
    }

    public ICommandParserBuilder UseConfiguration(ICommandParserConfiguration commandParserConfiguration)
    {
        _commandParserConfiguration = commandParserConfiguration;

        return this;
    }

    public ICommandParserBuilder UseArgumentTypeResolver<TResolver, TArgumentType>()
        where TResolver : IArgumentTypeResolver<TArgumentType>, new()
    {
        return UseArgumentTypeResolver(new TResolver());
    }

    public ICommandParserBuilder UseArgumentTypeResolver<TResolver, TArgumentType>(TResolver resolver)
        where TResolver : IArgumentTypeResolver<TArgumentType>
    {
        return UseArgumentTypeResolver(resolver);
    }

    public ICommandParserBuilder UseArgumentTypeResolver<TArgumentType>(IArgumentTypeResolver<TArgumentType> resolver)
    {
        _argumentTypeResolvers.Add(resolver ?? throw new ArgumentNullException(nameof(resolver)));

        return this;
    }

    public ICommandParser Build()
    {
        if (_commandParserConfiguration is null) throw new InvalidOperationException("Specify configuration first.");

        return new CommandParser(_commandParserConfiguration, _argumentTypeResolvers);
    }

    public ILocalizedCommandParser BuildLocalized(CommandNameRetrieverDelegate commandNameRetriever)
    {
        if (_commandParserConfiguration is null) throw new InvalidOperationException("Specify configuration first.");

        return new LocalizedCommandParser(_commandParserConfiguration, commandNameRetriever, _argumentTypeResolvers);
    }

    public static ICommandParser Default()
    {
        return new CommandParserBuilder().UseDefaultConfiguration().Build();
    }

    public static ILocalizedCommandParser DefaultLocalized(CommandNameRetrieverDelegate commandNameRetriever)
    {
        return new CommandParserBuilder().UseDefaultConfiguration().BuildLocalized(commandNameRetriever);
    }
}