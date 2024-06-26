using System;
using Bishop.Commands.Record.Business;
using Bishop.Helper;

namespace Bishop.Commands.Record.Controller;

public partial class RecordController
{
    public RecordFormatter Formatter { private get; set; } = new();
    public RecordManager Manager { private get; set; } = new();
    public PlotManager PlotManager { private get; set; } = null!;
    public Random Random { private get; set; } = null!;
    public IKeyBasedCache<ulong, string> Cache { private get; set; } = null!;
}