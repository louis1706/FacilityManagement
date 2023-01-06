using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesla = Exiled.API.Features.TeslaGate;

namespace FacilityManagement.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class DisableTesla : ICommand, IUsageProvider
    {
        public string Command { get; } = "disabletesla";
        public string[] Aliases { get; } = new string[] { "dt" };

        public string Description { get; } = "Disable the tesla (-1) to disable it entirely 0 for enable it";
        public string[] Usage { get; } = new string[]
        {
            "Duration",
        };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(!sender.CheckPermission(PlayerPermissions.FacilityManagement, out response))
            {
                return false;
            }
            if (!float.TryParse(arguments.ElementAtOrDefault(0), out float time))
            {
                response = $"Invalid duration time {arguments.ElementAtOrDefault(0)}";
                return false;
            }
            if (time < 0)
                time = float.MaxValue;
            foreach (Tesla tesla in Tesla.List)
            {
                tesla.InactiveTime = time;
            }
            return true;
        }
    }
}
