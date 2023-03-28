using System;
using System.Collections.Generic;

namespace Bishop.Commands.Record.Domain;

/// <summary>
///     Represent every possible category for <see cref="RecordEntity" />.
///     <remarks>ORDERS OF THE KEYS MUST NOT CHANGE AS ENUMS ARE CONVERTED TO INT WITH KEY POSITION</remarks>
/// </summary>
public enum CounterCategory
{
    Bdm = 0,
    Sauce = 1,
    Sel = 2,
    Beauf = 3,
    Rass = 4,
    Malfoy = 5,
    Wind = 6,
    Raclette = 7
}

public static class EnumRelations
{
    public static readonly Dictionary<CounterCategory, string> CounterCategoryToDisplayName = new()
    {
        {CounterCategory.Bdm, "BDM"},
        {CounterCategory.Sauce, "sauce"},
        {CounterCategory.Sel, "sel"},
        {CounterCategory.Beauf, "beauf"},
        {CounterCategory.Rass, "rass"},
        {CounterCategory.Malfoy, "malfoy"},
        {CounterCategory.Wind, "wind"},
        {CounterCategory.Raclette, "raclette"}
    };

    public static string DisplayName(this CounterCategory category)
    {
        return CounterCategoryToDisplayName.GetValueOrDefault(category) ?? throw new ArgumentNullException(nameof(category));
    }
}