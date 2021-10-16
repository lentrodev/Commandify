namespace Commandify.Abstractions;

public interface IArgumentTypeResolver
{}

public interface IArgumentTypeResolver<T> : IArgumentTypeResolver
{
    T Resolve(string value);
}