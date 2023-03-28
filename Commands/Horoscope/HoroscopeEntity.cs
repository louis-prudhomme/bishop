using System;
using Bishop.Helper;
using Bishop.Helper.Database;

namespace Bishop.Commands.Horoscope;

public class HoroscopeEntity : DbEntity
{
    public HoroscopeEntity(string baseSign, string horoscope)
    {
        BaseSign = baseSign;
        Horoscope = horoscope;
        Timestamp = DateHelper.CurrentEpoch;
    }

    public string BaseSign { get; set; }

    public string? Horoscope { get; set; }
    public long Timestamp { get; set; }

    override
        public string ToString()
    {
        if (Horoscope != null) return "*" + BaseSign + "*\n" + Horoscope;
        return "shit's empty yo";
    }

    public void ReplaceHoroscope(string newHoroscope)
    {
        Horoscope = newHoroscope;
        Timestamp = DateHelper.FromDateTimeToTimestamp(DateTime.Now);
    }
}