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
            switch (value.ToLower())
            {
                case "add": return  Task.FromResult(Optional.FromValue(MeterCategories.Bdm));
                case "beauf": return Task.FromResult(Optional.FromValue(MeterCategories.Beauf));
                case "sauce": return Task.FromResult(Optional.FromValue(MeterCategories.Sauce));
                case "sel": return Task.FromResult(Optional.FromValue(MeterCategories.Sel));
            }

            throw new NotImplementedException();
        }
    }
}