using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Record.Controller.Aliases;

public class WindCounterController : ApplicationCommandModule
{
    public RecordController Controller { private get; set; } = null!;

    [SlashCommand("rot", "Add a number of rot to someone")]
    public async Task ScoreRot(InteractionContext context,
        [OptionAttribute("user", "User to rots to")]
        DiscordUser user,
        [OptionAttribute("points", "How many rots ?")]
        [Maximum(10)]
        [Minimum(1)]
        long nb = 1)
    {
        await Controller.Score(context, user, CounterCategory.Rot, nb);
    }

    [SlashCommand("pet", "Add a number of pet to someone")]
    public async Task ScorePet(InteractionContext context,
        [OptionAttribute("user", "User to pets to")]
        DiscordUser user,
        [OptionAttribute("points", "How many pets ?")]
        [Maximum(10)]
        [Minimum(1)]
        long nb = 1)
    {
        await Controller.Score(context, user, CounterCategory.Pet, nb);
    }

    [ContextMenu(ApplicationCommandType.UserContextMenu, "Add a rot")]
    public async Task AddRot(ContextMenuContext context)
    {
        await Controller.AddMany(context, context.TargetUser, CounterCategory.Rot, 1);
    }

    [ContextMenu(ApplicationCommandType.UserContextMenu, "Add a pet")]
    public async Task AddPet(ContextMenuContext context)
    {
        await Controller.AddMany(context, context.TargetUser, CounterCategory.Rot, 1);
    }
}
