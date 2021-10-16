using Commandify.Abstractions;

namespace Commandify;

public class ArgumentTypeResolver<T> : IArgumentTypeResolver<T>
{
    private readonly ParseDelegate<T> _parseDelegate;

    public ArgumentTypeResolver(ParseDelegate<T> parseDelegate)
    {
        _parseDelegate = parseDelegate;
    }


    public ArgumentTypeResolver(TryParseDelegate<T> tryParseDelegate)
    {
        _parseDelegate = source =>
        {
            T value;

            if (tryParseDelegate(source, out value)) return value;

            return value;
        };
    }

    public T Resolve(string value)
    {
        return _parseDelegate(value);
    }
}