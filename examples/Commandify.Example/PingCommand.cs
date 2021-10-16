using System.Net.NetworkInformation;
using Commandify.Processing.Abstractions;

public class PingCommand : ICommandHandler<CommandContext, PingArguments>
{
    public static string Id => "ping";

    public static void Handle(CommandContext context, PingArguments args)
    {
        var ping = new Ping();

        try
        {
            var pingReply = ping.Send(args.Url, 1000);

            if (pingReply.Status == IPStatus.Success)
                Console.WriteLine(
                    $"Successfully pinged {args.Url}. It had took {TimeSpan.FromMilliseconds(pingReply.RoundtripTime)}");
            else
                Console.Write($"Fail :( Cannot ping {args.Url}. Reason: {pingReply.Status.ToString()}");
        }
        catch (Exception ex)
        {
            Console.Write(
                $"Fail :( Cannot ping {args.Url}. Exception message: {ex.InnerException?.Message ?? ex.Message}.");
        }
    }
}