﻿using System;
using System.Globalization;
using Bishop.Commands.Meter;
using Bishop.Helper;
using DSharpPlus.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Bishop.Commands.History
{
    /// <summary>
    /// Specifies and implements interactions of <see cref="RecordEntity"/> with DB.
    /// </summary>
    public class RecordEntity : DbObject
    {
        public RecordEntity() : base(false)
        {
            Motive = null!;
        }

        public RecordEntity(ulong discordMemberId, CountCategory category, string motive) : base(true)
        {
            RecordedAt = DateTime.Now;
            Timestamp = DateHelper.FromDateTimeToTimestamp(RecordedAt);
            
            UserId = discordMemberId;
            Category = category;
            Motive = motive;
        }
        
        //TODO remove me
        public RecordEntity(ulong discordMemberId, CountCategory category, Record record) : base(true)
        {
            RecordedAt = DateHelper.FromOldStringToDateTime(record.Date);
            Timestamp = DateHelper.FromDateTimeToTimestamp(RecordedAt);
            
            Motive = record.Motive;
            Category = category;
            UserId = discordMemberId;
        }
        
        public ulong UserId { get; set; }
        public CountCategory Category { get; set; }
        public DateTime RecordedAt { get; set; }
        public long Timestamp;
        public string Motive { get; set; }

        public override string ToString()
        {
            return $"*« {Motive} »* – {DateHelper.FromDateTimeToString(RecordedAt)}";
        }
    }
}