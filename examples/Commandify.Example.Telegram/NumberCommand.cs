using Commandify.Processing.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Commandify.Example.Telegram;

public class NumberCommand : ICommandHandler<TelegramCommandContext<Message>>
{
    public static string Id => "number";

    public static void Handle(TelegramCommandContext<Message> context)
    {
        var msg = context.Entity;
        
        context.Client.SendTextMessageAsync(msg.Chat, $"Select one command from 'decrease' and 'increase'. E.g number decrease 10");
    }
}