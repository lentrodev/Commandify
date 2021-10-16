# Commandify

Create commands and handle them easily with this library.

## Overview

**Commandify** is a lightweight and simple library to create, manage, and handle commands for C#.

Example below shows basic functional only for one simple command: 

<details>
    <summary>Example code</summary>

```c#
//Creating command instance
ICommand<EchoArguments> echoCommand = new Command<EchoArguments>("echo");

//Creating command parser with default confgiguration used
ICommandParser commandParser = CommandParserBuilder.Default();

while (true)
{
    string input = Console.ReadLine();
    
    //Parsing command text and arguments depending on EchoArguments class.
    ICommandParseResult<EchoArguments> echoCommandParseResult 
        = commandParser.Parse(echoCommand, input);

    if (echoCommandParseResult.Success)
    {
        Console.WriteLine($"Echo! Text: {echoCommandParseResult.Arguments.Text}");
    }
}

public class EchoArguments : IArguments
{
    [Argument]
    public string Text { get; set; }
}
```

</details>

Note, if you want to parse text with spaces as single argument,
wrap it in single or double quotes (e.g. `echo 'hello world!'`), then, you will have value `hello world!` in the `Text` property.

## Composing commands

**Commandify.Processing** provides additional functionality for composing commands, for reducing code repeating.

Example below shows how to configure and use two and more commands:

<details>
    <summary>Example code</summary>

```c#
ICommandProcessorBuilder<CommandContext> commandProcessorBuilder = new CommandProcessorBuilder<CommandContext>()
        .UseCommandParser(commandParserBuilder => commandParserBuilder
            .UseDefaultConfiguration()
        )
        .UseCommand<EchoArguments>("echo", HandleEchoCommand)
        .UseCommand<PingArguments>("ping", HandlePingCommand)
    ;

ICommandProcessor<CommandContext> commandProcessor 
    = commandProcessorBuilder.Build(_ => _.Text);

while (true)
{
    string input = Console.ReadLine();
    
    commandProcessor.Process(new CommandContext()
    {
        Text = input
    });
}

void HandleEchoCommand(CommandContext commandContext, EchoArguments args)
{
    Console.WriteLine($"Echo! Text: {args.Echo}");
}

void HandlePingCommand(CommandContext commandContext, PingArguments args)
{
    Ping ping = new Ping();

    try
    {
        PingReply pingReply = ping.Send(args.Url, 1000);
        
        if (pingReply.Status == IPStatus.Success)
        {
            Console.WriteLine($"Successfully pinged {args.Url}. It had took {TimeSpan.FromMilliseconds(pingReply.RoundtripTime)}");    
        }
        else
        {
            Console.Write($"Fail :( Cannot ping {args.Url}. Reason: {pingReply.Status.ToString()}");   
        }
    }
    catch (Exception ex)
    {
        Console.Write($"Fail :( Cannot ping {args.Url}. Exception message: {ex.InnerException?.Message ?? ex.Message}.");   
    }
}

public class EchoArguments : IArguments
{
    [Argument]
    public string Echo { get; set; }
}

public class PingArguments : IArguments
{
    [Argument]
    public string Url { get; set; }
}

public class CommandContext
{
    public string Text { get; set; }
}
```

</details>

Note, that you could also use interface `ICommandHandler<TContext>` and handling commands even more easy. For example, above example could rewritten in that way:

<details>
    <summary>Example code</summary>

```c#
ICommandProcessorBuilder<CommandContext> commandProcessorBuilder = new CommandProcessorBuilder<CommandContext>()
        .UseCommandParser(commandParserBuilder => commandParserBuilder
            .UseDefaultConfiguration()
        )
        .UseCommand<EchoCommand, EchoArguments>()
        .UseCommand<PingCommand, PingArguments>()
    ;

ICommandProcessor<CommandContext> commandProcessor
    = commandProcessorBuilder.Build(_ => _.Text);

while (true)
{
    string input = Console.ReadLine();

    commandProcessor.Process(new CommandContext()
    {
        Text = input
    });
}

public class EchoCommand : ICommandHandler<CommandContext, EchoArguments>
{
    public static string Id => "echo";

    public static void Handle(CommandContext context, EchoArguments args)
    {
        Console.WriteLine($"Echo! Text: {args.Echo}");
    }
}

public class PingCommand : ICommandHandler<CommandContext, PingArguments>
{
    public static string Id => "ping";

    public static void Handle(CommandContext context, PingArguments args)
    {
        Ping ping = new Ping();

        try
        {
            PingReply pingReply = ping.Send(args.Url, 1000);

            if (pingReply.Status == IPStatus.Success)
            {
                Console.WriteLine(
                    $"Successfully pinged {args.Url}. It had took {TimeSpan.FromMilliseconds(pingReply.RoundtripTime)}");
            }
            else
            {
                Console.Write($"Fail :( Cannot ping {args.Url}. Reason: {pingReply.Status.ToString()}");
            }
        }
        catch (Exception ex)
        {
            Console.Write(
                $"Fail :( Cannot ping {args.Url}. Exception message: {ex.InnerException?.Message ?? ex.Message}.");
        }
    }
}

public class EchoArguments : IArguments
{
    [Argument] public string Echo { get; set; }
}

public class PingArguments : IArguments
{
    [Argument] public string Url { get; set; }
}

public class CommandContext
{
    public string Text { get; set; }
}
```


</details>

