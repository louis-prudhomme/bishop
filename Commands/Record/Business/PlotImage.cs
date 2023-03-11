using System;
using Bishop.Helper;
using Plotly.NET;
using Plotly.NET.ImageExport;

namespace Bishop.Commands.Record.Business;

public class PlotImage
{
    private GenericChart.GenericChart _underlying;
    private Guid _guid = new();
    
    public PlotImage(GenericChart.GenericChart chart)
    {
        _underlying = chart;
        chart.SaveJPG(_guid.ToString());
    }

    public DisposableImage Image()
    {
        return new DisposableImage(_guid.ToString());
    }
}