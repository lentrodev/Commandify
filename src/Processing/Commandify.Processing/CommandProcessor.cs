using System.Globalization;
using System.Reflection;
using Commandify.Abstractions;
using Commandify.Processing.Abstractions;

namespace Commandify.Processing;

public record CommandDescriptor(ICommand Command, Delegate Handler);

public class CommandProcessor<TContext> : ICommandProcessor<TContext>
{
    private readonly ICommandParser _commandParser;
    private readonly IEnumerable<CommandDescriptor> _commands;
    private readonly CommandTextRetrieverDelegate<TContext> _commandTextRetriever;
    private readonly CultureInfoRetrieverDelegate<TContext> _cultureInfoRetriever;
    private readonly ILocalizedCommandParser _localizedCommandParser;
    private readonly bool _useCultureInfo;

    private CommandProcessor(IEnumerable<CommandDescriptor> commands,
        CommandTextRetrieverDelegate<TContext> commandTextRetriever)
    {
        _commands = commands;
        _commandTextRetriever = commandTextRetriever;
    }

    public CommandProcessor(ICommandParser commandParser, IEnumerable<CommandDescriptor> commands,
        CommandTextRetrieverDelegate<TContext> commandTextRetriever) : this(commands, commandTextRetriever)
    {
        _commandParser = commandParser;
        _useCultureInfo = false;
    }

    public CommandProcessor(ILocalizedCommandParser localizedCommandParser, IEnumerable<CommandDescriptor> commands,
        CommandTextRetrieverDelegate<TContext> commandTextRetriever,
        CultureInfoRetrieverDelegate<TContext> cultureInfoRetriever) : this(commands, commandTextRetriever)
    {
        _localizedCommandParser = localizedCommandParser;
        _cultureInfoRetriever = cultureInfoRetriever;
        _useCultureInfo = true;
    }

    public void Process(TContext context)
    {
        var commandText = _commandTextRetriever(context);

        var cultureInfo = _useCultureInfo ? _cultureInfoRetriever(context) : null;

        foreach (var commandDescriptor in _commands)
        {
            var command = commandDescriptor.Command;

            if (command.GetType().GetInterface(typeof(ICommand<>).Name) is { } genericInterface)
            {
                object[] args = { context, commandText, cultureInfo, command, commandDescriptor.Handler };

                GetType().GetMethod(nameof(ProcessCommandGeneric), BindingFlags.NonPublic | BindingFlags.Instance)
                    .MakeGenericMethod(genericInterface.GetGenericArguments().First())
                    .Invoke(this, args);
            }
            else
            {
                ProcessCommand(context, commandText, cultureInfo, command, commandDescriptor.Handler);
            }
        }
    }

    private void ProcessCommand(TContext context, string commandText, CultureInfo? cultureInfo, ICommand command,
        Delegate commandHandler)
    {
        if (_useCultureInfo && _localizedCommandParser.Parse(command, cultureInfo, commandText) is
            { Success: true } parseResult)
            commandHandler.DynamicInvoke(context);
        else if (_commandParser.Parse(command, commandText) is { Success: true }) commandHandler.DynamicInvoke(context);
    }

    private void ProcessCommandGeneric<T>(TContext context, string commandText, CultureInfo? cultureInfo,
        ICommand<T> command, Delegate commandHandler)
        where T : IArguments, new()
    {
        if (_useCultureInfo && _localizedCommandParser.Parse(command, cultureInfo, commandText) is
            { Success: true } result)
            commandHandler.DynamicInvoke(context, result.Arguments);
        else if (_commandParser.Parse(command, commandText) is { Success: true } result1)
            commandHandler.DynamicInvoke(context, result1.Arguments);
    }
}