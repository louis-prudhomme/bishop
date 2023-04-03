using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bishop.Helper.Extensions;

public static class StringExtensions
{
    /// <summary>
    ///     From https://stackoverflow.com/questions/1879395/how-do-i-generate-a-stream-from-a-string
    /// </summary>
    /// <param name="s">To streamify</param>
    /// <returns>a stream that can be `using`ed</returns>
    public static Stream ToStream(this string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    public static IEnumerable<string> SplitArguments(this string self)
    {
        return Regex
            .Matches(self, @"(?<match>\w+)|\""(?<match>[\w\s]*)""")
            .Select(m => m.Groups["match"].Value);
    }

    public static bool IsEmpty(this string s)
    {
        return s.Length == 0;
    }

    public static string IfEmpty(this string s, string placeholder)
    {
        return s.IsEmpty() ? placeholder : s;
    }
}