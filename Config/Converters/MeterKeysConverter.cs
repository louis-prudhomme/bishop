using System.Threading.Tasks;
using Bishop.Commands.Meter;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;

namespace Bishop.Config.Converters;

/// <summary>
///     This classes allows conversion of a DSharp command string argument to a <c>Keys</c> enum value.
///     It allows transparent use of the <c>Keys</c> enum as parameters of theses functions.
/// </summary>
internal class MeterKeysConverter : IArgumentConverter<CounterCategory>
{
    public Task<Optional<CounterCategory>> ConvertAsync(string value, CommandContext ctx)
    {
        return value.ToLower() switch
        {
            "bdm" => Task.FromResult(Optional.FromValue(CounterCategory.Bdm)),
            "beauf" => Task.FromResult(Optional.FromValue(CounterCategory.Beauf)),
            "sauce" => Task.FromResult(Optional.FromValue(CounterCategory.Sauce)),
            "sel" => Task.FromResult(Optional.FromValue(CounterCategory.Sel)),
            "rass" => Task.FromResult(Optional.FromValue(CounterCategory.Rass)),
            "maufoi" => Task.FromResult(Optional.FromValue(CounterCategory.Maufoi)),
            "maufoy" => Task.FromResult(Optional.FromValue(CounterCategory.Maufoi)),
            _ => Task.FromResult(Optional.FromNoValue<CounterCategory>())
        };
    }
}