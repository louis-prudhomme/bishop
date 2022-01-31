using System;
using System.Globalization;

namespace Bishop.Commands.History
{
    public class Record 
    {
        public Record(string motive)
        {
            Date = DateTime.Now.ToString(CultureInfo.CreateSpecificCulture("fr-FR"));
            
            Motive = motive;
        }

        public string Date { get; set; }
        public string Motive { get; set; }

        public override string ToString()
        {
            return $"*« {Motive} »* – {Date}";
        }
    }
}