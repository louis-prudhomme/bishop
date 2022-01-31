using Bishop.Commands.Helper;
using Google.Apis.Drive.v3.Data;

namespace Bishop.Commands.Meter
{
    public class CounterEntity : DbObject
    {
        public CounterEntity() : base(false)
        {
        }

        public CounterEntity(ulong user, CountCategory key) : base(true)
        {
            UserId = user;
            Key = key;
            Score = 0;
        }
        
        //TODO remove me
        public CounterEntity(ulong userId, Enumerat old) : base(true)
        {
            UserId = userId;
            Key = old.Key;
            Score = old.Score;
        }

        /// <summary>
        ///     Public Get/Set both necessary to deserialize from Mongo
        /// </summary>
        public ulong UserId { get; set; }

        public CountCategory Key { get; set; }
        public long Score { get; set; }

        public override string ToString()
        {
            return $"{UserId}’s {Key} ⇒ {Score}";
        }
    }
}