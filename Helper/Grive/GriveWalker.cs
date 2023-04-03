using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Helper.Extensions;

namespace Bishop.Helper.Grive;

// TODO make asynchronous ?
public delegate bool FileCheck(string path);

/// <summary>
///     Class which helps scrap files contained in the Grive folder.
///     The Grive folder path must be within environment variables.
///     Subfolder names are determined by <see cref="GriveDirectoryFormatter" />.
/// </summary>
public class GriveWalker
{
    public const long DiscordFileSizeLimitBytes = 8_000_000; // 8mb
    public const long CacheForSeconds = 14400;
    public static readonly List<string> AuthorizedExtensions = new() {".jpg", ".png", ".gif", ".mp4", ".webm", ".webp"};

    private static readonly string GrivePath = Environment
        .GetEnvironmentVariable("GRIVE_PATH")!;

    private readonly IEnumerable<FileCheck> _checks;

    public GriveWalker(IEnumerable<FileCheck> checks)
    {
        _checks = checks;
    }

    /// <summary>
    ///     Performs every check on a given path.
    /// </summary>
    /// <param name="path">To check</param>
    /// <returns></returns>
    private bool PerformAllChecks(string path)
    {
        return _checks
            .Select(check => check(path))
            .Aggregate(BooleanAdditions.And);
    }

    /// <summary>
    ///     Recursively finds all the files at a given path which conform to <see cref="_checks" />.
    /// </summary>
    /// <param name="path">To scrap.</param>
    /// <returns>All files contained and valid.</returns>
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

    private ImmutableList<string> FetchFiles(GriveDirectory directory)
    {
        return FetchFiles(GriveDirectoryFormatter.FormatToRelative(GrivePath, directory));
    }


    /// <summary>
    ///     Recursively finds all the files at a given path which conform to provided <see cref="_checks" />.
    ///     This is just a wrapper over a synchronous method.
    /// </summary>
    /// <param name="directory">To scrap.</param>
    /// <returns>All files contained and valid.</returns>
    public Task<ImmutableList<string>> FetchFilesAsync(GriveDirectory directory)
    {
        return Task.FromResult(FetchFiles(directory));
    }
}