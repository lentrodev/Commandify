using System.Linq.Expressions;
using Commandify.Processing.Abstractions;
using Telegram.Bot;

namespace Commandify.Example.Telegram;

public class TelegramCommandContext<TEntity> : ICommandContext
{
    private readonly Expression<Func<TEntity, string>> _textProperty;

    public TelegramCommandContext(TEntity entity, ITelegramBotClient client, Expression<Func<TEntity, string>> textProperty)
    {
        _textProperty = textProperty;
        Entity = entity;
        Client = client;
    }

    public static int Number { get; set; } = 0;

    public TEntity Entity { get; }

    public ITelegramBotClient Client { get; }

    public string Text
    {
        get
        {
            return _textProperty.Compile().Invoke(Entity);
        }
        set
        {
            string propertyName = ExpressionExtensions.GetName(_textProperty);
            
            Entity.GetType().GetProperty(propertyName).SetValue(Entity, value);
        }
    }


}