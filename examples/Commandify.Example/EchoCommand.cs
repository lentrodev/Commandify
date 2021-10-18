using Commandify.Processing.Abstractions;

namespace Commandify.Example;

public class EchoCommand : ICommandHandler<CommandContext, EchoArguments>
{
    public static string Id => "echo";

    public static void Handle(CommandContext context, EchoArguments args)
    {
        Console.WriteLine($"Echo! Text: {args.Echo}");
    }
}