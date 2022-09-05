using Bishop.Helper.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
///     Specifies and implements interactions of <see cref="HoroscopeEntity" /> with DB.
/// </summary>
namespace Bishop.Commands.Dump
{
    public class HoroscopeRepository : Repository<HoroscopeEntity>
    {
        private const string CollectionName = "horoscopes";

        public HoroscopeRepository() : base(CollectionName)
        {
        }
    }
}