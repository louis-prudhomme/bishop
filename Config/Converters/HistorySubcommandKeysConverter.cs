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
        public async Task<Optional<HistorySubcommandKey>> ConvertAsync(string value, CommandContext ctx)
        {
            switch (value.ToLower())
            {
                case "add": return HistorySubcommandKey.Add;
                case "see": return HistorySubcommandKey.See;
            }

            throw new NotImplementedException();
        }
    }
}