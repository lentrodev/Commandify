using Commandify;
using Commandify.Abstractions;
using Commandify.Builders;
using Commandify.Example;
using Commandify.Processing;

ICommandParser commandParser = new CommandParserBuilder()
    .UseDefaultConfiguration().Build();

var commandProcessorBuilder = new CommandProcessorBuilder<CommandContext>(commandParser)
        .UseCommand<EchoCommand, EchoArguments>()
        .UseCommand<PingCommand, PingArguments>()
    ;

var commandProcessor
    = commandProcessorBuilder.Build();

while (true)
{
    var commandText = Console.ReadLine();

    commandProcessor.Process(new CommandContext
    {
        Text = commandText
    });
}