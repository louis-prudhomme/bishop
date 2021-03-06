using System.IO;

namespace Bishop.Helper;

public static class StringExtensions
{
    /// <summary>
    /// From https://stackoverflow.com/questions/1879395/how-do-i-generate-a-stream-from-a-string
    /// </summary>
    /// <param name="s">To streamify</param>
    /// <returns>a stream that can be "<see cref="using"/>"ed</returns>
    public static Stream ToStream(this string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

}