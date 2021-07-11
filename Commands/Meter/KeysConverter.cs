﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bishop.Commands.Meter
{
    class KeysConverter : IArgumentConverter<Keys>
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
                        .ToUpper()
                        .Equals(value))));

            return Task.FromResult(Optional.FromNoValue<Keys>());
        }
    }
}
