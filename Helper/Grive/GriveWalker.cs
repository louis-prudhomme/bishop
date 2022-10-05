using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Helper.Extensions;

namespace Bishop.Helper.Grive;

public delegate bool FileCheck(string path);

public class GriveWalker
{
    public static readonly List<string> AuthorizedExtensions = new() {".jpg", ".png", ".gif"};
    public const long DiscordFileSizeLimitBytes = 8_000_000; // 8mb
    public const long CacheForSeconds = 14400;

    private static readonly string GrivePath = Environment
        .GetEnvironmentVariable("GRIVE_PATH")!;

    private readonly IEnumerable<FileCheck> _checks;

    public GriveWalker(IEnumerable<FileCheck> checks)
    {
        _checks = checks;
    }

    private bool PerformAllChecks(string path)
    {
        return _checks
            .Select(check => check(path))
            .Aggregate(BooleanAdditions.And);
    }


    private ImmutableList<string> FetchFiles(string path)
    {
        var files = Directory.EnumerateDirectories(path)
            .Select(dir => Path.Join(path, dir))
            .SelectMany(FetchFiles)
            .ToList();

        files.AddRange(Directory
            .EnumerateFiles(path)
            .Where(PerformAllChecks));

        return files.ToImmutableList();
    }

    private ImmutableList<string> FetchFiles(GriveDirectory directory) =>
        FetchFiles(GriveDirectoryFormatter.FormatToRelative(GrivePath, directory));

    public Task<ImmutableList<string>> FetchFilesAsync(GriveDirectory directory) =>
        Task.FromResult(FetchFiles(directory));
}