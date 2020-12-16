using Synapse.Command;


namespace Scp106Rework.Commands
{
    [CommandInformation(
        Name = "Pocket",
        Aliases = new[] { "po" },
        Description = "A Command which allows Scp-106 to go into his Pocket",
        Permission = "",
        Platforms = new[] { Platform.ClientConsole },
        Usage = ".pocket"
        )]
    public class PocketCommand : ISynapseCommand
    {
        public CommandResult Execute(CommandContext context)
        {
            if (context.Player.RoleType != RoleType.Scp106)
                return new CommandResult
                {
                    Message = "You are not Scp106",
                    State = CommandResultState.NoPermission
                };

            if (!PluginClass.Config.PocketLarry) return new CommandResult
            {
                Message = "The Command is disabled on this Server",
                State = CommandResultState.Error
            };

            if (context.Player.GetComponent<Scp106ReworkScript>().DoPocketAnimation())
                return new CommandResult
                {
                    Message = "You are now going into your Pocket",
                    State = CommandResultState.Ok
                };

            return new CommandResult
            {
                Message = "The Cooldown is active so please wait until the Animation is done",
                State = CommandResultState.Ok
            };
        }
    }
}
