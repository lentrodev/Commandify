using Commandify.Abstractions;

namespace Commandify.Processing.Abstractions;

public interface ICommandHandler<TContext>
{
    static abstract string Id { get; }

    static abstract void Handle(TContext context);
}

public interface ICommandHandler<TContext, TArguments> where TArguments : IArguments, new()
{
    static abstract string Id { get; }

    static abstract void Handle(TContext context, TArguments args);
}