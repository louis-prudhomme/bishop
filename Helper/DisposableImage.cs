using System;
using System.IO;
using Microsoft.VisualBasic;

namespace Bishop.Helper;

public class DisposableImage : IDisposable
{
    private const string Path = "./";
    private readonly string _filename;
    private bool _disposed;

    public DisposableImage(string filename)
    {
        _filename = filename;
    }

    public string Filepath => Path + Filename;
    public string Filename => $"{_filename}.jpg";
    public string FilenameWithoutExtension => _filename;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public FileStream Stream()
    {
        return File.Open(Filepath, FileMode.Open);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            FileSystem.Kill(Filepath);
            File.Delete(Filepath);
        }

        _disposed = true;
    }


    ~DisposableImage()
    {
        Dispose(false);
    }
}