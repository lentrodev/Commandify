using System.Globalization;

namespace Commandify.Abstractions;

public delegate string CommandNameRetrieverDelegate(string commandId, CultureInfo cultureInfo);