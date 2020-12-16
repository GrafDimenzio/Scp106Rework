using Synapse.Api.Plugin;

namespace Scp106Rework
{
    //Original PocketTrap: https://github.com/sanyae2439/PocketTrap
    [PluginInformation(
        Name = "Scp106Rework",
        Author = "Dimenzio",
        Description = "A Plugin that adds a bunch of new features to Scp106",
        LoadPriority = int.MinValue,
        SynapseMajor = 2,
        SynapseMinor = 2,
        SynapsePatch = 0,
        Version = "v.1.0.0"
        )]
    public class PluginClass : AbstractPlugin
    {
        [Config(section = "Scp106Rework")]
        public static PluginConfig Config;

        public override void Load() => new EventHandlers();
    }
}
