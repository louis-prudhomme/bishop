using System.Threading.Tasks;
using Bishop.Commands.Helper;
using Bishop.Commands.Meter;
using MongoDB.Driver;

namespace Bishop.Commands.Meter
{
    public class CounterRepository : Repository<CounterEntity>
    {
        private const string CollectionName = "counter";

        public CounterRepository() : base(CollectionName)
        {
        }
    }
}