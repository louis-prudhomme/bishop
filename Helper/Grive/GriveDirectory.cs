using System.IO;
using DSharpPlus;

namespace Bishop.Helper.Grive;

public enum GriveDirectory
{
    Pigtures
}

public static class GriveDirectoryFormatter
{
    public static string Format(GriveDirectory directory) => directory.ToString("G").ToLower();

    public static string FormatToRelative(string path, GriveDirectory directory) => Path.Join(path, Format(directory));
}