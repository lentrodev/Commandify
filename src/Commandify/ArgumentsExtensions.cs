using System.Reflection;
using Commandify.Abstractions;
using Commandify.Abstractions.Attributes;

namespace Commandify;

public static class ArgumentsExtensions
{
    public static IEnumerable<ArgumentDescriptor> ValidateArguments<TArguments>() where TArguments : IArguments
    {
        var propertyInfos = typeof(TArguments).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var propertyInfo in propertyInfos)
            if (propertyInfo.GetCustomAttribute<ArgumentAttribute>() is { } attribute &&
                propertyInfo.GetSetMethod() is { })
                yield return new ArgumentDescriptor(propertyInfo, attribute);
    }
}