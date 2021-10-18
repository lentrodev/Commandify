using Commandify.Abstractions;
using Commandify.Abstractions.Builders;

namespace Commandify.Processing.Abstractions;

public interface ICommandProcessorBuilder<TContext>
    where TContext : ICommandContext
{
    ICommandProcessorBuilder<TContext> UseCommand(string commandId, CommandHandlerDelegate<TContext> commandHandler);

    ICommandProcessorBuilder<TContext> UseCommand<TArguments>(string commandId,
        CommandHandlerDelegate<TContext, TArguments> commandHandler) where TArguments : IArguments, new();

    ICommandProcessorBuilder<TContext> UseCommand<TCommandHandler>() where TCommandHandler : ICommandHandler<TContext>;

    ICommandProcessorBuilder<TContext> UseCommand<TCommandHandler, TArguments>()
        where TCommandHandler : ICommandHandler<TContext, TArguments> where TArguments : IArguments, new();


    ICommandProcessorBuilder<TContext> UseCommand(string commandId,
        ConfigureCommandProcessorDelegate<TContext> configureCommandProcessor,
        CommandHandlerDelegate<TContext> commandHandler = default,
        HandlerInvocationMode invocationMode = HandlerInvocationMode.SubCommandNotFound,
        HandlerInvocationOrder invocationOrder = HandlerInvocationOrder.After);


    ICommandProcessorBuilder<TContext> UseCommand<TArguments>(string commandId,
        ConfigureCommandProcessorDelegate<TContext> configureCommandProcessor,
        CommandHandlerDelegate<TContext, TArguments> commandHandler = default,
        HandlerInvocationMode invocationMode = HandlerInvocationMode.SubCommandNotFound,
        HandlerInvocationOrder invocationOrder = HandlerInvocationOrder.After) where TArguments : IArguments, new();


    ICommandProcessorBuilder<TContext> UseCommand<TCommandHandler>(
        ConfigureCommandProcessorDelegate<TContext> configureCommandProcessor,
        HandlerInvocationMode invocationMode = HandlerInvocationMode.SubCommandNotFound,
        HandlerInvocationOrder invocationOrder = HandlerInvocationOrder.After)
        where TCommandHandler : ICommandHandler<TContext>;

    ICommandProcessorBuilder<TContext> UseCommand<TCommandHandler, TArguments>(
        ConfigureCommandProcessorDelegate<TContext> configureCommandProcessor,
        HandlerInvocationMode invocationMode = HandlerInvocationMode.SubCommandNotFound,
        HandlerInvocationOrder invocationOrder = HandlerInvocationOrder.After)
        where TCommandHandler : ICommandHandler<TContext, TArguments> where TArguments : IArguments, new();


    ICommandProcessor<TContext> Build();
}