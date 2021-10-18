namespace Commandify.Processing.Abstractions;

public delegate void ConfigureCommandProcessorDelegate<TContext>(
    ICommandProcessorBuilder<TContext> commandProcessorBuilder) where TContext : ICommandContext;