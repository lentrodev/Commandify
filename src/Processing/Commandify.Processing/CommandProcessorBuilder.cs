using System.Dynamic;
using Commandify.Abstractions;
using Commandify.Abstractions.Builders;
using Commandify.Builders;
using Commandify.Processing.Abstractions;

namespace Commandify.Processing;

public class CommandProcessorBuilder<TContext> : ICommandProcessorBuilder<TContext>
    where TContext : ICommandContext
{
    internal readonly List<CommandDescriptor> _commandDescriptors;
    private readonly bool _useLocalizedParser = false;
    private ICommandParser _commandParser;
    private CultureInfoRetrieverDelegate<TContext> _cultureInfoRetriever;
    private ILocalizedCommandParser _localizedCommandParser;

    private CommandProcessorBuilder()
    {
        _commandDescriptors = new List<CommandDescriptor>();
    }

    public CommandProcessorBuilder( ICommandParser commandParser) : this()
    {
        _commandParser = commandParser;
    }

    public CommandProcessorBuilder(
        ILocalizedCommandParser commandParser, CultureInfoRetrieverDelegate<TContext> cultureInfoRetrieverDelegate) :
        this()
    {
        _useLocalizedParser = true;
        _localizedCommandParser = commandParser;
        _cultureInfoRetriever = cultureInfoRetrieverDelegate;
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

    public ICommandProcessorBuilder<TContext> UseCommand(string commandId,
        ConfigureCommandProcessorDelegate<TContext> configureCommandProcessor,
        CommandHandlerDelegate<TContext> commandHandler = default,
        HandlerInvocationMode invocationMode = HandlerInvocationMode.SubCommandNotFound,
        HandlerInvocationOrder invocationOrder = HandlerInvocationOrder.After)
    {
        ICommandProcessorBuilder<TContext> commandProcessorBuilder = _useLocalizedParser ? 
            new CommandProcessorBuilder<TContext>(_localizedCommandParser, _cultureInfoRetriever) :
            new CommandProcessorBuilder<TContext>(_commandParser);

        configureCommandProcessor(commandProcessorBuilder);

        var childProcessor = commandProcessorBuilder.Build();

        void InvokeInner(TContext context)
        {
            if (invocationOrder is HandlerInvocationOrder.Before)
            {
                if (invocationMode is HandlerInvocationMode.SubCommandNotFound)
                {
                    throw new InvalidOperationException(
                        "Cannot use both HandlerInvocationMode.SubCommandNotFound and HandlerInvocationOrder.Before");
                }

                commandHandler?.Invoke(context);
            }

            bool subCommandMatched = childProcessor.Process(context);

            if (invocationOrder is HandlerInvocationOrder.After)
            {
                if (invocationMode is HandlerInvocationMode.SubCommandNotFound && !subCommandMatched)
                {
                    commandHandler?.Invoke(context);
                }
                else if (invocationMode is HandlerInvocationMode.Always)
                {
                    commandHandler?.Invoke(context);
                }
            }
        }

        return UseCommand(commandId, InvokeInner);
    }

    public ICommandProcessorBuilder<TContext> UseCommand<TArguments>(string commandId,
        ConfigureCommandProcessorDelegate<TContext> configureCommandProcessor,
        CommandHandlerDelegate<TContext, TArguments> commandHandler = default,
        HandlerInvocationMode invocationMode = HandlerInvocationMode.SubCommandNotFound,
        HandlerInvocationOrder invocationOrder = HandlerInvocationOrder.After) where TArguments : IArguments, new()
    {
        ICommandProcessorBuilder<TContext> commandProcessorBuilder = _useLocalizedParser ? 
            new CommandProcessorBuilder<TContext>(_localizedCommandParser, _cultureInfoRetriever) :
            new CommandProcessorBuilder<TContext>(_commandParser);

        configureCommandProcessor(commandProcessorBuilder);

        var childProcessor = commandProcessorBuilder.Build();

        void InvokeInner(TContext context, TArguments arguments)
        {
            if (invocationOrder is HandlerInvocationOrder.Before)
            {
                if (invocationMode is HandlerInvocationMode.SubCommandNotFound)
                {
                    throw new InvalidOperationException(
                        "Cannot use both HandlerInvocationMode.SubCommandNotFound and HandlerInvocationOrder.Before");
                }

                commandHandler?.Invoke(context, arguments);
            }

            bool subCommandMatched = childProcessor.Process(context);

            if (invocationOrder is HandlerInvocationOrder.After)
            {
                if (invocationMode is HandlerInvocationMode.SubCommandNotFound && !subCommandMatched)
                {
                    commandHandler?.Invoke(context, arguments);
                }
                else if (invocationMode is HandlerInvocationMode.Always)
                {
                    commandHandler?.Invoke(context, arguments);
                }
            }
        }

        return UseCommand<TArguments>(commandId, InvokeInner);
    }

    public ICommandProcessorBuilder<TContext> UseCommand<TCommandHandler>(
        ConfigureCommandProcessorDelegate<TContext> configureCommandProcessor,
        HandlerInvocationMode invocationMode = HandlerInvocationMode.SubCommandNotFound,
        HandlerInvocationOrder invocationOrder = HandlerInvocationOrder.After)
        where TCommandHandler : ICommandHandler<TContext>
    {
        return UseCommand(TCommandHandler.Id, configureCommandProcessor, TCommandHandler.Handle, invocationMode,
            invocationOrder);
    }

    public ICommandProcessorBuilder<TContext> UseCommand<TCommandHandler, TArguments>(
        ConfigureCommandProcessorDelegate<TContext> configureCommandProcessor,
        HandlerInvocationMode invocationMode = HandlerInvocationMode.SubCommandNotFound,
        HandlerInvocationOrder invocationOrder = HandlerInvocationOrder.After)
        where TCommandHandler : ICommandHandler<TContext, TArguments> where TArguments : IArguments, new()
    {
        return UseCommand<TArguments>(TCommandHandler.Id, configureCommandProcessor, TCommandHandler.Handle,
            invocationMode,
            invocationOrder);
    }

    public ICommandProcessor<TContext> Build()
    {
        if (_useLocalizedParser)
            return new CommandProcessor<TContext>(_localizedCommandParser, _commandDescriptors,
                _cultureInfoRetriever);

        return new CommandProcessor<TContext>(_commandParser, _commandDescriptors);
    }
}