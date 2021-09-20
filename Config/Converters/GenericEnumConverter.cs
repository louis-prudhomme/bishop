using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;

namespace Bishop.Config.Converters
{
    public class GenericEnumConverter<T> : IArgumentConverter<T>
    {
        public async Task<Optional<T>> ConvertAsync(string value, CommandContext ctx)
        {
            try
            {
                if (!typeof(T).IsEnum)
                    throw new NotImplementedException();

                // get all the possible enum values and compare them with the parameter
                // if any match, true
                if (Enum.IsDefined(typeof(T), value))
                    return Optional.FromValue((T) Enum.Parse(typeof(T), value));

                return Optional.FromNoValue<T>();
            }
            catch (Exception e)
            {
                await ctx.RespondAsync(e.Message);
                return Optional.FromNoValue<T>();
            }
        }
    }
}