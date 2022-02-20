using Synapse.Api.Plugin;

namespace Scp106Rework
{
    //Original PocketTrap: https://github.com/sanyae2439/PocketTrap
    //Original Stalk: https://github.com/RogerFK/stalky106
    //Original BetterSinkhole: https://github.com/rby-blackruby/BetterSinkholes
    [PluginInformation(
        Name = "Scp106Rework",
        Author = "Dimenzio",
        Description = "A Plugin that adds a bunch of new features to SCP-106",
        LoadPriority = 0,
        SynapseMajor = 2,
        SynapseMinor = 9,
        SynapsePatch = 0,
        Version = "v.1.1.1"
        )]
    public class PluginClass : AbstractPlugin
    {
        [Config(section = "Scp106Rework")]
        public static PluginConfig Config;

        public override void Load()
        {
            new EventHandlers();
            base.Load();
        }
    }
}
