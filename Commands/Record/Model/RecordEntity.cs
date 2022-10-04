﻿using System;
using Bishop.Helper;
using Bishop.Helper.Database;

namespace Bishop.Commands.Record.Model;

/// <summary>
///     Specifies and implements interactions of <see cref="RecordEntity" /> with DB.
/// </summary>
public class RecordEntity : DbEntity
{
    public RecordEntity(ulong discordMemberId, CounterCategory category, string? motive)
    {
        RecordedAt = DateTime.Now;
        Timestamp = DateHelper.FromDateTimeToTimestamp(RecordedAt);

        UserId = discordMemberId;
        Category = category;
        Motive = motive;
    }

    public ulong UserId { get; set; }
    public CounterCategory Category { get; set; }
    public DateTime RecordedAt { get; set; }
    public string? Motive { get; set; }
    public long Timestamp { get; set; }
}