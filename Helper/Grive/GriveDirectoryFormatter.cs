using System.IO;

namespace Bishop.Helper.Grive;

public static class GriveDirectoryFormatter
{
    private static string Format(GriveDirectory directory) => directory.ToString("G").ToLower();

    public static string FormatToRelative(string path, GriveDirectory directory) => Path.Join(path, Format(directory));
}