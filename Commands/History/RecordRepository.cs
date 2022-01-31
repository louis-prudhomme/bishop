using Bishop.Helper;

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