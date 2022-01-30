using System.Threading.Tasks;
using Bishop.Commands.Helper;
using Bishop.Commands.Meter;
using MongoDB.Driver;

namespace Bishop.Commands.History
{
    public class RecordRepository : Repository<RecordEntity>
    {
        private const string CollectionName = "history";

        public RecordRepository() : base(CollectionName)
        {
        }
    }
}