namespace Commandify.Abstractions;

public interface ICommand<TArguments> : ICommand
    where TArguments : IArguments, new()
{}