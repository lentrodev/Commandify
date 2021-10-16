using System.Text.RegularExpressions;

namespace Commandify;

internal enum StringSegmentTrimMode
{
    None,
    Quote,
    DoubleQuote
}

internal record CommandDataSegment(string Value, StringSegmentTrimMode TrimMode);

internal class CommandDataSegmentCollection
{
    private readonly CommandDataSegment[] _sourceSegments;

    public CommandDataSegmentCollection(string source, string splitRegex, bool useQuote, bool useDoubleQuote)
    {
        CurrentIndex = 0;

        var segments = Regex.Matches(source, splitRegex)
            .Select(_ =>
            {
                CommandDataSegment segmentValue = null;

                if (useQuote && _.Value.StartsWith("\'") && _.Value.EndsWith("\'"))
                    segmentValue = new CommandDataSegment(_.Value.Trim('\''), StringSegmentTrimMode.Quote);

                if (useDoubleQuote && _.Value.StartsWith("\"") && _.Value.EndsWith("\""))
                    segmentValue = new CommandDataSegment(_.Value.Trim('"'), StringSegmentTrimMode.Quote);

                segmentValue ??= new CommandDataSegment(_.Value, StringSegmentTrimMode.None);

                return segmentValue;
            });

        _sourceSegments = segments.ToArray();
    }

    public int CurrentIndex { get; private set; }

    public bool SegmentsAvailable => CurrentIndex < _sourceSegments.Length;

    public override string ToString()
    {
        var segments = _sourceSegments[CurrentIndex..];

        var segmentsArray = new string[segments.Length];

        for (var i = 0; i < segments.Length; i++)
        {
            var segment = segments[i];

            var segmentValue = segment.Value;

            var strToAdd = segment.TrimMode is StringSegmentTrimMode.Quote ? "\'" :
                segment.TrimMode is StringSegmentTrimMode.DoubleQuote ? "\"" : "";

            segmentValue = $"{strToAdd}{segmentValue}{strToAdd}";

            segmentsArray[i] = segmentValue;
        }

        return string.Join(" ", segmentsArray);
    }

    public string Take()
    {
        return _sourceSegments[CurrentIndex++].Value;
    }
}