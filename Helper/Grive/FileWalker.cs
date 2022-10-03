using System;
using System.IO;

namespace Bishop.Helper.Grive;

public class FileWalker
{
    private static readonly string GrivePath = Environment
        .GetEnvironmentVariable("GRIVE_PATH")!;
    
    public static void DisplayGriveContent()
    {
        var p = Directory.GetDirectories(".");
        var p1 = Directory.GetFiles(".");
        var p2 = Directory.GetFiles("./Resources");
        var p3 = Directory.GetDirectories("./Resources");
        var z = Directory.GetCurrentDirectory();
        var a = Directory.GetParent(".");
        Console.WriteLine();
    }
}