using Synapse.Config;
using System.Collections.Generic;
using System.ComponentModel;

namespace Scp106Rework
{
    public class PluginConfig : AbstractConfigSection
    {
        [Description("If Enabled all Exits are an Escape if no Scp-106 exists")]
        public bool AlwaysEscapeWithoutLarry = true;

        [Description("If Enabled the CustomExits from below will be used instead of the normal ones")]
        public bool CustomExits = false;

        [Description("The CustomExits where the Victims of 106 exits the pocket dimension")]
        public List<SerializedMapPoint> CustomExitPoints = new List<SerializedMapPoint>
        {
            new SerializedMapPoint("HCZ_106",2f,6f,0f),
            new SerializedMapPoint("HCZ_106",15f,6f,0f),
        };

        [Description("The exit location of the Pocket after the Warhead explodes")]
        public SerializedMapPoint WarheadExit = new SerializedMapPoint("Root_*&*Outside Cams", -54f, -8f, -59f);

        [Description("The perecentage of how likely it is that a Player get's a second chanche to escape the pocket")]
        public float SecondTryChanche = 80f;

        [Description("The Damage the player gets when he gets the chanche the escape a second time")]
        public int SecondTryDamage = 25;

        [Description("If Enabled the Ragdolls of the victims of Scp-106 spawns outside of the Pocket")]
        public bool SpawnRagdollOutside = true;

        [Description("If Enabled Scp-106 can bring other Scp's into the pocket")]
        public bool PocketScps = true;

        [Description("If Enabled Scp-106 can in his Own Pocket")]
        public bool PocketLarry = true;

        [Description("If Enabled Humans can fall through the portal of Scp-106 in the Pocket")]
        public bool PocketTrap = false;

        [Description("If Enabled Scp-106 can stalk to a random player")]
        public bool Stalk = true;

        [Description("The Initial Cooldown of stalk")]
        public float StalkSpawnCooldown = 100f;

        [Description("The Cooldown after Scp-106 has used his stalk abillity")]
        public float StalkCooldown = 60f;

        [Description("If Sinkholes in the Light can teleport players into the pocket")]
        public bool BetterSinkhole = true;

        [Description("The Distance from the Sinkhole where you get teleported")]
        public float SinkholeTeleportDistance = 3f;
    }
}
