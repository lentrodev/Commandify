using CommandLine;

namespace Commandify.Example.Telegram;

public class TelegramBotOptions
{
    [Option('t', "token", HelpText = "Your Telegram Bot Token.")]
    public string ApiToken { get; set; }
}