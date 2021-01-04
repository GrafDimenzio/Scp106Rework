using Synapse.Command;

namespace Scp106Rework.Commands
{
    [CommandInformation(
        Name = "PocketScp",
        Aliases = new[] { "ps" },
        Description = "A Command that sends other Scp's into the Pocket",
        Permission = "",
        Platforms = new[] { Platform.ClientConsole },
        Usage = ".ps and while using the Command look at a Scp"
        )]
    public class PocketScpCommand : ISynapseCommand
    {
        public CommandResult Execute(CommandContext context)
        {
            if (context.Player.RoleType != RoleType.Scp106)
                return new CommandResult
                {
                    Message = "You are not Scp106",
                    State = CommandResultState.NoPermission,
                };

            if (!PluginClass.Config.PocketScps)
                return new CommandResult
                {
                    Message = "The Command is disabled on this Server",
                    State = CommandResultState.Error
                };

            var player = context.Player.LookingAt?.GetComponent<Synapse.Api.Player>();

            if (player == null)
                return new CommandResult
                {
                    Message = "No Player was found",
                    State = CommandResultState.Error
                };

            if (player.RealTeam != Team.SCP)
                return new CommandResult
                {
                    Message = "You can only bring Scp's with this Command in the Pocket",
                    State = CommandResultState.Error
                };

            if (player.RoleType == RoleType.Scp106) return new CommandResult
            {
                Message = "You can't bring Scp106 into the Pocket",
                State = CommandResultState.Error,
            };

            player.Position = UnityEngine.Vector3.up * -1997f;
            return new CommandResult
            {
                Message = "Scp was brought into the Pocket",
                State = CommandResultState.Ok,
            };
        }
    }
}
