using Bishop.Helper;

namespace Bishop.Commands.History;

public class RecordRepository : Repository<RecordEntity>
{
    private const string CollectionName = "records";

    public RecordRepository() : base(CollectionName)
    {
    }
}