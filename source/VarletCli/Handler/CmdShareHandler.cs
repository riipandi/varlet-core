﻿using McMaster.Extensions.CommandLineUtils;
using Variety;

namespace VarletCli.Handler
{
    [Command("share", Description = "Share your site publicly")]
    public class CmdShareHandler
    {
        private void OnExecute(IConsole console)
        {
            console.WriteLine($"\nThis is share command\n");
        }
    }
}
