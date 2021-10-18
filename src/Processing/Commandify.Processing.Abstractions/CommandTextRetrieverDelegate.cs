namespace Commandify.Processing.Abstractions;

public delegate string CommandTextRetrieverDelegate<TContext>(TContext context)where TContext : ICommandContext;