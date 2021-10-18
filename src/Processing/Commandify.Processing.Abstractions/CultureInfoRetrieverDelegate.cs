using System.Globalization;

namespace Commandify.Processing.Abstractions;

public delegate CultureInfo CultureInfoRetrieverDelegate<TContext>(TContext context)where TContext : ICommandContext;