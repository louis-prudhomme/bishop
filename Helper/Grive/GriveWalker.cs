using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace Bishop.Helper.Grive;

public record GriveCache(ImmutableList<string> Files, long LastFetched);

public class GriveWalker
{
    //TODO check duration
    private const long CacheFor = 14400;

    private static readonly string GrivePath = Environment
        .GetEnvironmentVariable("GRIVE_PATH")!;

    private readonly ConcurrentDictionary<GriveDirectory, GriveCache> _filesCache = new();

    private static ImmutableList<string> FetchFiles(string path)
    {
        var files = Directory.EnumerateDirectories(path)
            .Select(dir => Path.Join(path, dir))
            .SelectMany(FetchFiles)
            .ToList();
        files.AddRange(Directory.EnumerateFiles(path));
        return files.ToImmutableList();
    }

    private static GriveCache FetchFilesInDirectory(GriveDirectory directory)
    {
        return new GriveCache(FetchFiles(GriveDirectoryFormatter.FormatToRelative(GrivePath, directory)),
            DateHelper.CurrentEpoch);
    }

    private void BuildGriveDirectoryCache(GriveDirectory directory)
    {
        _filesCache.AddOrUpdate(directory,
            FetchFilesInDirectory,
            (_, files) => files);
    }

    public void BuildGriveCache()
    {
        foreach (var directory in Enum.GetValues<GriveDirectory>())
            BuildGriveDirectoryCache(directory);
    }

    public ImmutableList<string> GetFiles(GriveDirectory directory)
    {
        var (files, lastFetched) = _filesCache.GetOrAdd(directory,
            _ => new GriveCache(new List<string>().ToImmutableList(), 0));

        if (DateHelper.CurrentEpoch - lastFetched <= CacheFor) return files;

        var newValue = FetchFilesInDirectory(directory);
        _filesCache.AddOrUpdate(directory,
            FetchFilesInDirectory,
            (_, newFiles) => newFiles);
        return newValue.Files;
    }
}