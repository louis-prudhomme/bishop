using System;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.History;
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
    internal class HistorySubcommandKeysConverter : IArgumentConverter<HistorySubcommandKey>
    {
        public Task<Optional<HistorySubcommandKey>> ConvertAsync(string value, CommandContext ctx)
        {
            try
            {
                // get all the possible enum values and compare them with the parameter
                // if any match, true
                var isKey = Enum.GetValues(typeof(MeterCategories)).OfType<MeterCategories>()
                    .Select(key => key.ToString())
                    .Select(key => key.ToLower())
                    .Any(key => value.ToLower().Equals(key));

                // if match, then find the value that matches 
                // cannot do this in one step, as the default value for enum is 0
                if (isKey)
                    return Task.FromResult(Optional.FromValue(Enum
                        .GetValues(typeof(HistorySubcommandKey))
                        .OfType<HistorySubcommandKey>()
                        .FirstOrDefault(key => key.ToString()
                            .ToLower().Equals(value))));

                return Task.FromResult(Optional.FromNoValue<HistorySubcommandKey>());
            }
            catch (Exception e)
            {
                ctx.RespondAsync(e.Message);
                throw new Exception();
            }
        }
    }
}