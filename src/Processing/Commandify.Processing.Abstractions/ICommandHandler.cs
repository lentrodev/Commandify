using Commandify.Abstractions;

namespace Commandify.Processing.Abstractions;

public interface ICommandHandler<TContext>
    where TContext : ICommandContext
{
    static abstract string Id { get; }

    static abstract void Handle(TContext context);
}

public interface ICommandHandler<TContext, TArguments>
    where TContext : ICommandContext
    where TArguments : IArguments, new()
{
    static abstract string Id { get; }

    static abstract void Handle(TContext context, TArguments args);
}