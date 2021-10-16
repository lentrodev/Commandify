namespace Commandify.Processing.Abstractions;

public delegate void CommandHandlerDelegate<TContext>(TContext context);

public delegate void CommandHandlerDelegate<TContext, TArguments>(TContext context, TArguments arguments);