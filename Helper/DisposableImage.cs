using System;
using System.IO;
using System.Reflection;
using log4net;
using Microsoft.VisualBasic;

namespace Bishop.Helper;

public class DisposableImage : IDisposable
{
    private const string Path = "./";

    public string Filepath => Path + Filename;
    public string Filename => $"{_filename.ToString()}.jpg";
    public string FilenameWithoutExtension => _filename.ToString();
    private readonly string _filename;
    private bool _disposed = false;

    public DisposableImage(string filename)
    {
        _filename = filename;
    }

    public FileStream Stream() => File.Open(Filepath, FileMode.Open);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
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