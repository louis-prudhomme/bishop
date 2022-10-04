using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace Bishop.Helper.Grive;

public class GriveWalker
{
    private static readonly string GrivePath = Environment
        .GetEnvironmentVariable("GRIVE_PATH")!;

    private readonly ConcurrentDictionary<GriveDirectory, ImmutableList<string>> _files = new();

    private static ImmutableList<string> FetchFiles(string path)
    {
        var files = Directory.EnumerateDirectories(path)
            .Select(dir => Path.Join(path, dir))
            .SelectMany(FetchFiles)
            .ToList();
        files.AddRange(Directory.EnumerateFiles(path));
        return files.ToImmutableList();
    }

    private void BuildGriveDirectoryCache(GriveDirectory directory)
    {
        _files.AddOrUpdate(directory,
            dir => FetchFiles(GriveDirectoryFormatter.FormatToRelative(GrivePath, dir)),
            (_, files) => files);
    }

    public void BuildGriveCache()
    {
        foreach (var directory in Enum.GetValues<GriveDirectory>())
            BuildGriveDirectoryCache(directory);
    }
}