using Commandify.Processing;

var commandProcessorBuilder = new CommandProcessorBuilder<CommandContext>()
        .UseCommandParser(commandParserBuilder => commandParserBuilder
            .UseDefaultConfiguration()
        )
        .UseCommand<EchoCommand, EchoArguments>()
        .UseCommand<PingCommand, PingArguments>()
    ;

var commandProcessor
    = commandProcessorBuilder.Build(_ => _.Text);

while (true)
{
    var input = Console.ReadLine();

    commandProcessor.Process(new CommandContext
    {
        Text = input
    });
}