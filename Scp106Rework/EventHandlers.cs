using MEC;
using Synapse;
using Synapse.Api;
using Synapse.Api.Enum;
using System.Linq;
using UnityEngine;
using Ev = Synapse.Api.Events.EventHandler;

namespace Scp106Rework
{
    public class EventHandlers
    {
        public EventHandlers()
        {
            Ev.Get.Scp.Scp106.PocketDimensionLeaveEvent += ExitPocket;
            Ev.Get.Player.PlayerKeyPressEvent += KeyPress;
            Ev.Get.Player.LoadComponentsEvent += Load;
            Ev.Get.Scp.Scp106.PortalCreateEvent += CreatePortal;
            Ev.Get.Player.PlayerJoinEvent += Join;
            Ev.Get.Server.UpdateEvent += Update;
            Ev.Get.Player.PlayerSetClassEvent += SetClass;
            Ev.Get.Player.PlayerWalkOnSinkholeEvent += Sinkhole;
            Ev.Get.Scp.ScpAttackEvent += ScpAttack;
        }

        private void Update()
        {
            if (!PluginClass.Config.PocketTrap) return;

            FindPortal();

            if (portal != null)
                foreach(var ply in Server.Get.Players)
                {
                    if (Vector3.Distance(ply.Position, portal.transform.position) <= 2.5 && ply.RealTeam != Team.SCP && portal.transform.position != Vector3.zero)
                        ply.GetComponent<Scp106ReworkScript>().DoPocketTrapAnimation(false);
                }
        }

        private void ScpAttack(Synapse.Api.Events.SynapseEventArguments.ScpAttackEventArgs ev)
        {
            if (ev.AttackType == ScpAttackType.Scp106_Grab && ev.Target.Zone == ZoneType.Pocket && !PluginClass.Config.DimensionAttack)
                ev.Allow = false;
        }

        private void Sinkhole(Synapse.Api.Events.SynapseEventArguments.PlayerWalkOnSinkholeEventArgs ev)
        {
            if (ev.Allow && PluginClass.Config.BetterSinkhole && Vector3.Distance(ev.Player.Position, ev.Sinkhole.transform.position) <= PluginClass.Config.SinkholeTeleportDistance)
                ev.Player.GetComponent<Scp106ReworkScript>().DoPocketTrapAnimation(true);
        }

        private void SetClass(Synapse.Api.Events.SynapseEventArguments.PlayerSetClassEventArgs ev)
        {
            if (ev.Role == RoleType.Scp106) ev.Player.GetComponent<Scp106ReworkScript>().stalkCanBeUsedTime = Time.time + PluginClass.Config.StalkSpawnCooldown;
        }

        private void Join(Synapse.Api.Events.SynapseEventArguments.PlayerJoinEventArgs ev) => MEC.Timing.CallDelayed(1f,() => RefreshPortal());

        private void CreatePortal(Synapse.Api.Events.SynapseEventArguments.PortalCreateEventArgs ev)
        {
            if (ev.Scp106.Scp106Controller.IsUsingPortal) return;

            ev.Scp106.GetComponent<Scp106ReworkScript>().Stalk(true);

            if(ev.Scp106.Room.Zone == ZoneType.Pocket)
            {
                ev.Allow = false;
                ev.Scp106.GiveTextHint("You can't create a portal in your Pocket.");
                return;
            }
        }

        private void Load(Synapse.Api.Events.SynapseEventArguments.LoadComponentEventArgs ev)
        {
            if (ev.Player.GetComponent<Scp106ReworkScript>() == null)
                ev.Player.AddComponent<Scp106ReworkScript>();
        }

        private void KeyPress(Synapse.Api.Events.SynapseEventArguments.PlayerKeyPressEventArgs ev)
        {
            if (ev.Player.RoleType != RoleType.Scp106) return;

            switch (ev.KeyCode)
            {
                case KeyCode.Alpha1:
                    if (!PluginClass.Config.PocketScps) return;
                    var player = ev.Player.LookingAt?.GetPlayer();
                    if (player == null) return;
                    if (player.RealTeam != Team.SCP || player.RoleType == RoleType.Scp106) return;
                    player.Position = Vector3.up * -1997;
                    break;

                case KeyCode.Alpha2:
                    ev.Player.GetComponent<Scp106ReworkScript>().DoPocketAnimation();
                    break;

                case KeyCode.Alpha3:
                    ev.Player.GetComponent<Scp106ReworkScript>().Stalk(false);
                    break;
            }
        }

        private void ExitPocket(Synapse.Api.Events.SynapseEventArguments.PocketDimensionLeaveEventArgs ev)
        {
            if (ev.Player.RealTeam == Team.SCP)
                ev.TeleportType = PocketDimensionTeleport.PDTeleportType.Exit;

            if (PluginClass.Config.AlwaysEscapeWithoutLarry && !Server.Get.Players.Any(x => x.RoleType == RoleType.Scp106))
                ev.TeleportType = PocketDimensionTeleport.PDTeleportType.Exit;

            if (Map.Get.Nuke.Detonated)
                ev.ExitPosition = PluginClass.Config.WarheadExit.Parse().Position;
            else if (PluginClass.Config.CustomExits)
                ev.ExitPosition = PluginClass.Config.CustomExitPoints.ElementAt(Random.Range(0, PluginClass.Config.CustomExitPoints.Count)).Parse().Position;

            if(ev.TeleportType == PocketDimensionTeleport.PDTeleportType.Killer)
            {
                if(Random.Range(1f, 100f) <= PluginClass.Config.SecondTryChanche)
                {
                    ev.Allow = false;
                    ev.Player.Position = Vector3.up * -1997;
                    ev.Player.Hurt(PluginClass.Config.SecondTryDamage, DamageType.PocketDecay);
                    ev.Player.GiveTextHint("You didn't found the exit but you have survied it!");
                    return;
                }
                if (PluginClass.Config.SpawnRagdollOutside)
                {
                    ev.Allow = false;
                    ev.Player.Position = ev.ExitPosition;
                    var killer = Server.Get.Players.FirstOrDefault(x => x.Scp106Controller.PocketPlayers.Contains(ev.Player));
                    if (killer == null)
                        killer = ev.Player;
                    Timing.CallDelayed(0.1f, () => ev.Player.Hurt(99999999, DamageType.PocketDecay));
                }
            }
        }

        private GameObject portal;

        private void RefreshPortal()
        {
            if (!PluginClass.Config.PocketTrap) return;

            FindPortal();

            var player = Server.Get.Players.FirstOrDefault();
            if (player == null)
                player = Server.Get.Host;

            player.Scp106Controller.PortalPosition = portal.transform.position;
        } 

        private void FindPortal()
        {
            if(portal == null)
                portal = GameObject.Find("SCP106_PORTAL");
        }
    }
}
