using System.Globalization;
using System.Reflection;
using Commandify.Abstractions;
using Commandify.Processing.Abstractions;

namespace Commandify.Processing;

public class CommandProcessor<TContext> : ICommandProcessor<TContext>
    where TContext : ICommandContext
{
    private readonly ICommandParser _commandParser;
    private readonly IEnumerable<CommandDescriptor> _commands;
    private readonly CultureInfoRetrieverDelegate<TContext> _cultureInfoRetriever;
    private readonly ILocalizedCommandParser _localizedCommandParser;
    private readonly bool _useCultureInfo;

    private CommandProcessor(IEnumerable<CommandDescriptor> commands)
    {
        _commands = commands;
    }

    public CommandProcessor(ICommandParser commandParser, IEnumerable<CommandDescriptor> commands) : this(commands)
    {
        _commandParser = commandParser;
        _useCultureInfo = false;
    }

    public CommandProcessor(ILocalizedCommandParser localizedCommandParser, IEnumerable<CommandDescriptor> commands,
        CultureInfoRetrieverDelegate<TContext> cultureInfoRetriever) : this(commands)
    {
        _localizedCommandParser = localizedCommandParser;
        _cultureInfoRetriever = cultureInfoRetriever;
        _useCultureInfo = true;
    }

    public bool Process(TContext context)
    {
        var commandText = context.Text;

        var cultureInfo = _useCultureInfo ? _cultureInfoRetriever(context) : null;

        foreach (var commandDescriptor in _commands)
        {
            var command = commandDescriptor.Command;

            bool isMatch = true;
            
            if (command.GetType().GetInterface(typeof(ICommand<>).Name) is { } genericInterface)
            {
                object[] args = { context, commandText, cultureInfo, command, commandDescriptor.Handler };

                isMatch = (bool)GetType().GetMethod(nameof(ProcessCommandGeneric), BindingFlags.NonPublic | BindingFlags.Instance)
                    .MakeGenericMethod(genericInterface.GetGenericArguments().First())
                    .Invoke(this, args);
            }
            else
            {
                isMatch = ProcessCommand(context, commandText, cultureInfo, command, commandDescriptor.Handler);
            }

            if (isMatch)
                return true;
        }

        return false;
    }

    private bool ProcessCommand(TContext context, string commandText, CultureInfo? cultureInfo, ICommand command,
        Delegate commandHandler)
    {
        if (_useCultureInfo && _localizedCommandParser.Parse(command, cultureInfo, commandText) is
            { Success: true,ResultText: {} resultText })
        {
            context.Text = resultText;
            
            commandHandler.DynamicInvoke(context);

            return true;
        }
        if (_commandParser?.Parse(command, commandText) is { Success: true, ResultText: {} resultText1 })
        {
            context.Text = resultText1;
            
            commandHandler.DynamicInvoke(context);
            
            return true;
        }

        return false;
    }

    private bool ProcessCommandGeneric<T>(TContext context, string commandText, CultureInfo? cultureInfo,
        ICommand<T> command, Delegate commandHandler)
        where T : IArguments, new()
    {
        if (_useCultureInfo && _localizedCommandParser.Parse(command, cultureInfo, commandText) is
            { Success: true, ResultText: {} resultText, Arguments: {} args })
        {
            context.Text = resultText;
            
            commandHandler.DynamicInvoke(context, args);
            
            return true;
        }
        else if (_commandParser?.Parse(command, commandText) is { Success: true, ResultText: {} resultText1, Arguments: {} args1 })
        {
            context.Text = resultText1;
            
            commandHandler.DynamicInvoke(context, args1);
            
            return true;
        }

        return false;
    }
}