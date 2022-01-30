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
    internal class MeterKeysConverter : IArgumentConverter<MeterCategory>
    {
        public Task<Optional<MeterCategory>> ConvertAsync(string value, CommandContext ctx)
        {
            return value.ToLower() switch
            {
                "add" => Task.FromResult(Optional.FromValue(MeterCategory.Bdm)),
                "beauf" => Task.FromResult(Optional.FromValue(MeterCategory.Beauf)),
                "sauce" => Task.FromResult(Optional.FromValue(MeterCategory.Sauce)),
                "sel" => Task.FromResult(Optional.FromValue(MeterCategory.Sel)),
                "rass" => Task.FromResult(Optional.FromValue(MeterCategory.Rass)),
                _ => Task.FromResult(Optional.FromNoValue<MeterCategory>())
            };
        }
    }
}