using Autofac;
using Commandify.Abstractions.Builders;
using Commandify.Builders;

namespace Commandify.DependencyInjection.Autofac;

public static class ContainerBuilderExtensions
{
    public static ContainerBuilder RegisterCommandProcessor(this ContainerBuilder containerBuilder)
    {
        ICommandProcessorBuilder
    }

    public static ContainerBuilder RegisterCommandParser(this ContainerBuilder containerBuilder, Action<ICommandParserBuilder> configureCommandParser)
    {
        ICommandParserBuilder commandParserBuilder = new CommandParserBuilder();
    }
}