using Commandify.Abstractions;

namespace Commandify.Processing;

public record CommandDescriptor(ICommand Command, Delegate Handler);