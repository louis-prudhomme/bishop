﻿using System.Threading.Tasks;
using Bishop.Commands.Meter;
using Bishop.Helper;
using MongoDB.Driver;

namespace Bishop.Commands.Meter
{
    /// <summary>
    /// Specifies and implements interactions of <see cref="CounterEntity"/> with DB.
    /// </summary>
    public class CounterRepository : Repository<CounterEntity>
    {
        private const string CollectionName = "counter";

        public CounterRepository() : base(CollectionName)
        {
        }
    }
}