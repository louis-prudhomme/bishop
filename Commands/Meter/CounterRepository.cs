using Bishop.Helper;

namespace Bishop.Commands.Meter
{
    /// <summary>
    ///     Specifies and implements interactions of <see cref="CounterEntity" /> with DB.
    /// </summary>
    public class CounterRepository : Repository<CounterEntity>
    {
        private const string CollectionName = "counter";

        public CounterRepository() : base(CollectionName)
        {
        }
    }
}