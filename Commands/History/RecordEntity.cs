using System;
using System.Globalization;
using Bishop.Commands.Helper;
using Bishop.Commands.Meter;
using DSharpPlus.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Bishop.Commands.History
{
    public class RecordEntity : DbObject
    {
        public RecordEntity() : base(false) {}

        public RecordEntity(ulong discordMemberId, MeterCategories category, string motive) : base(true)
        {
            RecordedAt = DateTime.Now;
            Timestamp = DateHelper.FromDateTimeToTimestamp(RecordedAt);
            
            DiscordMemberId = discordMemberId;
            Category = category;
            Motive = motive;
        }
        
        //TODO remove me
        public RecordEntity(ulong discordMemberId, MeterCategories category, Record record) : base(true)
        {
            RecordedAt = DateHelper.FromOldStringToDateTime(record.Date);
            Timestamp = DateHelper.FromDateTimeToTimestamp(RecordedAt);
            
            Motive = record.Motive;
            Category = category;
            DiscordMemberId = discordMemberId;
        }
        
        public ulong DiscordMemberId { get; set; }
        public MeterCategories Category { get; set; }
        public DateTime RecordedAt { get; set; }
        public long Timestamp;
        public string Motive { get; set; }

        public override string ToString()
        {
            return $"*« {Motive} »* – {DateHelper.FromDateTimeToString(RecordedAt)}";
        }
    }
}