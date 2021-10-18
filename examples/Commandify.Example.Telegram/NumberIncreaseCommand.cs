using Commandify.Processing.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Commandify.Example.Telegram;

public class NumberIncreaseCommand : ICommandHandler<TelegramCommandContext<Message>, NumberArgs>
{
    public static string Id => "increase";

    public static void Handle(TelegramCommandContext<Message> context, NumberArgs args)
    {
        var msg = context.Entity;

        TelegramCommandContext<Message>.Number += args.Amount;
        
        context.Client.SendTextMessageAsync(msg.Chat, $"Increased by {args.Amount}. Current value: {TelegramCommandContext<Message>.Number}");
    }
}