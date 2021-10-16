using Commandify.Abstractions;

namespace Commandify;

public class Command : ICommand
{
    public Command(string id)
    {
        Id = id;
    }

    public string Id { get; }
}

public class Command<TArguments> : Command, ICommand<TArguments> where TArguments : IArguments, new()
{
    public Command(string id) : base(id)
    {
    }
}