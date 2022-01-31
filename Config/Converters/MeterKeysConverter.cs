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
    internal class MeterKeysConverter : IArgumentConverter<CountCategory>
    {
        public Task<Optional<CountCategory>> ConvertAsync(string value, CommandContext ctx)
        {
            return value.ToLower() switch
            {
                "add" => Task.FromResult(Optional.FromValue(CountCategory.Bdm)),
                "beauf" => Task.FromResult(Optional.FromValue(CountCategory.Beauf)),
                "sauce" => Task.FromResult(Optional.FromValue(CountCategory.Sauce)),
                "sel" => Task.FromResult(Optional.FromValue(CountCategory.Sel)),
                "rass" => Task.FromResult(Optional.FromValue(CountCategory.Rass)),
                _ => Task.FromResult(Optional.FromNoValue<CountCategory>())
            };
        }
    }
}