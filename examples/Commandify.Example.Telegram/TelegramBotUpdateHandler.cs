using Commandify.Processing.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace Commandify.Example.Telegram;

public class TelegramBotUpdateHandler : IUpdateHandler
{
    private readonly ICommandProcessor<TelegramCommandContext<Message>> _messageCommandProcessor;

    public TelegramBotUpdateHandler(ICommandProcessor<TelegramCommandContext<Message>> messageCommandProcessor)
    {
        _messageCommandProcessor = messageCommandProcessor;
    }

    public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var telegramCommandContext = new TelegramCommandContext<Message>(update.Message, botClient);

        Task.Run(() => _messageCommandProcessor.Process(telegramCommandContext), cancellationToken);

        return Task.CompletedTask;
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"An error has occured: {exception.Message}");

        return Task.CompletedTask;
    }
}