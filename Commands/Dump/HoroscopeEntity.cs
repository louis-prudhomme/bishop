using Bishop.Helper;
using Bishop.Helper.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bishop.Commands.Dump
{
    public class HoroscopeEntity : DbEntity
    {
        public HoroscopeEntity(string baseSign, string horoscope)
        {
            this.baseSign = baseSign;
            this.horoscope = horoscope;
            this.timestamp = DateHelper.FromDateTimeToTimestamp(DateTime.Now);
        }

        public string baseSign { get; set; }

        public string horoscope { get; set; }
        public long timestamp { get; set; }

        override
        public string ToString()
        {
            if (this.horoscope != null)
            {
                return "*" + this.baseSign + "*\n" + this.horoscope;
            }
            return "shit's empty yo";
        }

        public void ReplaceHoroscope(string newHoroscope)
        {
            this.horoscope = newHoroscope;
            this.timestamp = DateHelper.FromDateTimeToTimestamp(DateTime.Now);
        }
    }
}
