using System.Reflection;
using Commandify.Abstractions.Attributes;

namespace Commandify;

public struct ArgumentDescriptor
{
    public ArgumentDescriptor(PropertyInfo propertyInfo, ArgumentAttribute attribute)
    {
        PropertyInfo = propertyInfo;
        Attribute = attribute;
    }

    public PropertyInfo PropertyInfo { get; }

    public ArgumentAttribute Attribute { get; }
}