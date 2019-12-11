﻿using McMaster.Extensions.CommandLineUtils;
using Variety;

namespace VarletCli
{
    [Command("stop", Description = "Stop Varlet services")]
    public class CmdStopHandler
    {
        private void OnExecute(IConsole console)
        {
            console.WriteLine($"\nThis is stop command\n");
        }
    }
}
