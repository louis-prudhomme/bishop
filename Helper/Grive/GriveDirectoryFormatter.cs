using System.IO;

namespace Bishop.Helper.Grive;

/// <summary>
///     Helper class which formats <see cref="GriveDirectory" /> to string.
/// </summary>
public static class GriveDirectoryFormatter
{
    /// <summary>
    ///     Formats the <see cref="GriveDirectory" /> to string using its enum name, in lowercase.
    /// </summary>
    /// <param name="directory">Directory to format.</param>
    /// <returns>String value of the <see cref="GriveDirectory" /></returns>
    private static string Format(GriveDirectory directory)
    {
        return directory.ToString("G").ToLower();
    }

    /// <summary>
    ///     Returns a formatted path, including the given <see cref="GriveDirectory" />.
    /// </summary>
    /// <param name="path">Base path to iterate upon.</param>
    /// <param name="directory">To format and add to the path.</param>
    /// <returns>Formatted path.</returns>
    public static string FormatToRelative(string path, GriveDirectory directory)
    {
        return Path.Join(path, Format(directory));
    }
}