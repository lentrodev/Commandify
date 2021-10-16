﻿using System.Globalization;
using Commandify.Abstractions;
using Commandify.Abstractions.Builders;

namespace Commandify.Processing.Abstractions;

public delegate string CommandTextRetrieverDelegate<TContext>(TContext context);

public delegate CultureInfo CultureInfoRetrieverDelegate<TContext>(TContext context);

public interface ICommandProcessorBuilder<TContext>
{
    ICommandProcessorBuilder<TContext> UseCultureInfoRetriever(
        CultureInfoRetrieverDelegate<TContext> cultureInfoRetriever);

    ICommandProcessorBuilder<TContext> UseCommandParser(Action<ICommandParserBuilder> configureCommandParser);

    ICommandProcessorBuilder<TContext> UseLocalizedCommandParser(Action<ICommandParserBuilder> configureCommandParser,
        CommandNameRetrieverDelegate commandNameRetriever);

    ICommandProcessorBuilder<TContext> UseCommand(string commandId, CommandHandlerDelegate<TContext> commandHandler);

    ICommandProcessorBuilder<TContext> UseCommand<TArguments>(string commandId,
        CommandHandlerDelegate<TContext, TArguments> commandHandler) where TArguments : IArguments, new();

    ICommandProcessorBuilder<TContext> UseCommand<TCommandHandler>() where TCommandHandler : ICommandHandler<TContext>;

    ICommandProcessorBuilder<TContext> UseCommand<TCommandHandler, TArguments>()
        where TCommandHandler : ICommandHandler<TContext, TArguments> where TArguments : IArguments, new();

    ICommandProcessor<TContext> Build(CommandTextRetrieverDelegate<TContext> commandTextRetriever);
}