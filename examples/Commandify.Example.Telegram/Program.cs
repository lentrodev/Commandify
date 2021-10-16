using Commandify.Abstractions.Builders;
using Commandify.Example.Telegram;
using Commandify.Processing;
using Commandify.Processing.Abstractions;
using CommandLine;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

await Parser.Default.ParseArguments<TelegramBotOptions>(args)
    .WithParsedAsync(StartBotAsync);

Task StartBotAsync(TelegramBotOptions telegramBotOptions)
{
    ITelegramBotClient telegramBotClient = new TelegramBotClient(telegramBotOptions.ApiToken);

    ICommandProcessorBuilder<TelegramCommandContext<Message>> commandProcessorBuilder =
        new CommandProcessorBuilder<TelegramCommandContext<Message>>();

    ConfigureCommandProcessor(commandProcessorBuilder);

    CommandTextRetrieverDelegate<TelegramCommandContext<Message>> commandTextRetriever = message => message.Entity.Text;

    var commandProcessor = commandProcessorBuilder.Build(commandTextRetriever);

    var telegramBotUpdateHandler = new TelegramBotUpdateHandler(commandProcessor);

    return telegramBotClient.ReceiveAsync(telegramBotUpdateHandler, new ReceiverOptions
    {
        AllowedUpdates = new[]
        {
            UpdateType.Message
        }
    });
}

void ConfigureCommandProcessor(ICommandProcessorBuilder<TelegramCommandContext<Message>> commandProcessorBuilder)
{
    commandProcessorBuilder
        .UseCommandParser(ConfigureCommandParser)
        .UseCommand<EchoCommand>();
}

void ConfigureCommandParser(ICommandParserBuilder commandParserBuilder)
{
    commandParserBuilder
        .UseDefaultConfiguration();
}