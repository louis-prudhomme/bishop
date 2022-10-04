using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bishop.Helper.Grive;

public record GriveCache(ImmutableList<string> Files, long LastFetched);

public class GriveWalker
{
    //TODO check duration
    public const long CacheFor = 14400;
    public IKeyBasedCache<GriveDirectory, ImmutableList<string>> FilesCache { private get; set; } = null!;

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

    public static ImmutableList<string> FetchFiles(GriveDirectory directory) =>
        FetchFiles(GriveDirectoryFormatter.FormatToRelative(GrivePath, directory));

    public static Task<ImmutableList<string>> FetchFilesAsync(GriveDirectory directory) =>
        Task.FromResult(FetchFiles(directory));

    public void BuildGriveCache()
    {
        foreach (var directory in Enum.GetValues<GriveDirectory>())
            FilesCache.Get(directory);
    }
}