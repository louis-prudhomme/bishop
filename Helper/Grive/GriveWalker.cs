using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bishop.Helper.Grive;


public static class GriveWalker
{
    public const long CacheForSeconds = 14400;

    private static readonly string GrivePath = Environment
        .GetEnvironmentVariable("GRIVE_PATH")!;

    private static ImmutableList<string> FetchFiles(string path)
    {
        var files = Directory.EnumerateDirectories(path)
            .Select(dir => Path.Join(path, dir))
            .SelectMany(FetchFiles)
            .ToList();
        files.AddRange(Directory.EnumerateFiles(path));
        return files.ToImmutableList();
    }

    private static ImmutableList<string> FetchFiles(GriveDirectory directory) =>
        FetchFiles(GriveDirectoryFormatter.FormatToRelative(GrivePath, directory));

    public static Task<ImmutableList<string>> FetchFilesAsync(GriveDirectory directory) =>
        Task.FromResult(FetchFiles(directory));
}