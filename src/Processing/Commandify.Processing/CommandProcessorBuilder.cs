using Commandify.Abstractions;
using Commandify.Abstractions.Builders;
using Commandify.Builders;
using Commandify.Processing.Abstractions;

namespace Commandify.Processing;

public class CommandProcessorBuilder<TContext> : ICommandProcessorBuilder<TContext>
{
    private readonly List<CommandDescriptor> _commandDescriptors;
    private readonly bool _useLocalizedParser = false;
    private ICommandParser _commandParser;
    private CultureInfoRetrieverDelegate<TContext> _cultureInfoRetriever;
    private ILocalizedCommandParser _localizedCommandParser;

    public CommandProcessorBuilder()
    {
        _commandDescriptors = new List<CommandDescriptor>();
    }

    public ICommandProcessorBuilder<TContext> UseCultureInfoRetriever(
        CultureInfoRetrieverDelegate<TContext> cultureInfoRetriever)
    {
        _cultureInfoRetriever = cultureInfoRetriever;

        return this;
    }

    public ICommandProcessorBuilder<TContext> UseCommandParser(Action<ICommandParserBuilder> configureCommandParser)
    {
        ICommandParserBuilder commandParserBuilder = new CommandParserBuilder();

        configureCommandParser(commandParserBuilder);

        var commandParser = commandParserBuilder.Build();

        _commandParser = commandParser;

        return this;
    }

    public ICommandProcessorBuilder<TContext> UseLocalizedCommandParser(
        Action<ICommandParserBuilder> configureCommandParser, CommandNameRetrieverDelegate commandNameRetriever)
    {
        ICommandParserBuilder commandParserBuilder = new CommandParserBuilder();

        configureCommandParser(commandParserBuilder);

        var commandParser = commandParserBuilder.BuildLocalized(commandNameRetriever);

        _localizedCommandParser = commandParser;

        return this;
    }

    public ICommandProcessorBuilder<TContext> UseCommand(string commandId,
        CommandHandlerDelegate<TContext> commandHandler)
    {
        _commandDescriptors.Add(new CommandDescriptor(new Command(commandId), commandHandler));

        return this;
    }

    public ICommandProcessorBuilder<TContext> UseCommand<TArguments>(string commandId,
        CommandHandlerDelegate<TContext, TArguments> commandHandler) where TArguments : IArguments, new()
    {
        _commandDescriptors.Add(new CommandDescriptor(new Command<TArguments>(commandId), commandHandler));

        return this;
    }

    public ICommandProcessorBuilder<TContext> UseCommand<TCommandHandler>()
        where TCommandHandler : ICommandHandler<TContext>
    {
        return UseCommand(TCommandHandler.Id, TCommandHandler.Handle);
    }

    public ICommandProcessorBuilder<TContext> UseCommand<TCommandHandler, TArguments>()
        where TCommandHandler : ICommandHandler<TContext, TArguments> where TArguments : IArguments, new()
    {
        return UseCommand<TArguments>(TCommandHandler.Id, TCommandHandler.Handle);
    }

    public ICommandProcessor<TContext> Build(CommandTextRetrieverDelegate<TContext> commandTextRetriever)
    {
        if (_useLocalizedParser)
            return new CommandProcessor<TContext>(_localizedCommandParser, _commandDescriptors, commandTextRetriever,
                _cultureInfoRetriever);

        return new CommandProcessor<TContext>(_commandParser, _commandDescriptors, commandTextRetriever);
    }
}