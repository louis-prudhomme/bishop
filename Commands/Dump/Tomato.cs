﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Dump
{
    /// <summary>
    ///     Provide a command to send tomatoes to @users.
    /// </summary>
    public class Tomato : BaseCommandModule
    {
        public Random Rand { private get; set; }
        public static List<string> Tomatoes { get; set; }

        [Command("tomato")]
        [Aliases("t")]
        [Description("Throw a tomato to @someone")]
        public async Task Throw(CommandContext context,
            [Description("User to throw the tomato at!")]
            DiscordMember member)
        {
            await context.RespondAsync($"{member.Mention} 🍅 ! {Tomatoes[Rand.Next(Tomatoes.Count)]}");
        }
    }
}