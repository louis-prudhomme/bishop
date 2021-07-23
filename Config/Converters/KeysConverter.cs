using System;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.Meter;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;

namespace Bishop.Config.Converters
{
    internal class KeysConverter : IArgumentConverter<Keys>
    {
        public Task<Optional<Keys>> ConvertAsync(string value, CommandContext ctx)
        {
            var isKey = Enum.GetValues(typeof(Keys)).OfType<Keys>()
                .Any(key => key.ToString().Equals(value.ToUpper()));

            if (isKey)
                return Task.FromResult(Optional.FromValue(Enum
                    .GetValues(typeof(Keys))
                    .OfType<Keys>()
                    .FirstOrDefault(key => key
                        .ToString()
                        .Equals(value.ToUpper()))));

            return Task.FromResult(Optional.FromNoValue<Keys>());
        }
    }
}