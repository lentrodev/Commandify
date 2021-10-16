namespace Commandify.Processing.Abstractions;

public interface ICommandProcessor<TContext>
{
    void Process(TContext context);
}