using Commandify.Processing.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Commandify.Example.Telegram;

public class EchoCommand : ICommandHandler<TelegramCommandContext<Message>>
{
    public static string Id => "echo";

    public static void Handle(TelegramCommandContext<Message> context)
    {
        var msg = context.Entity;

        Console.WriteLine($"{msg.From.FirstName} {msg.From.LastName} - {msg.Text}");

        context.Client.SendTextMessageAsync(msg.Chat, msg.Text);
    }
}