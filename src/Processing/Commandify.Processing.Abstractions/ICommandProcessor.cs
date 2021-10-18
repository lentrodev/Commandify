namespace Commandify.Processing.Abstractions;

public interface ICommandProcessor<TContext>
    where TContext : ICommandContext
{
    bool Process(TContext context);
}