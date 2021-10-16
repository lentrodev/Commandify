using Telegram.Bot;

namespace Commandify.Example.Telegram;

public class TelegramCommandContext<TEntity>
{
    public TelegramCommandContext(TEntity entity, ITelegramBotClient client)
    {
        Entity = entity;
        Client = client;
    }

    public TEntity Entity { get; }

    public ITelegramBotClient Client { get; }
}