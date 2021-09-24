using System;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.Meter;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;

namespace Bishop.Config.Converters
{
    /// <summary>
    ///     This classes allows conversion of a DSharp command string argument to a <c>Keys</c> enum value.
    ///     It allows transparent use of the <c>Keys</c> enum as parameters of theses functions.
    /// </summary>
    internal class MeterKeysConverter : IArgumentConverter<MeterCategories>
    {
        public Task<Optional<MeterCategories>> ConvertAsync(string value, CommandContext ctx)
        {
             var t = ctx.RespondAsync(value).Result;
            return value.ToLower() switch
            {
                "add" => Task.FromResult(Optional.FromValue(MeterCategories.Bdm)),
                "beauf" => Task.FromResult(Optional.FromValue(MeterCategories.Beauf)),
                "sauce" => Task.FromResult(Optional.FromValue(MeterCategories.Sauce)),
                "sel" => Task.FromResult(Optional.FromValue(MeterCategories.Sel)),
                _ => Task.FromResult(Optional.FromNoValue<MeterCategories>())
            };
        }
    }
}