using UnityEngine;
using Synapse.Api;
using Synapse;
using System.Linq;
using System.Collections.Generic;

namespace Scp106Rework
{
    public class Scp106ReworkScript : MonoBehaviour
    {
        private readonly Player player;

        public Scp106ReworkScript() => player = GetComponent<Player>();

        #region PocketLarry
        private Vector3 oldPos = Vector3.zero;

        public bool DoPocketAnimation()
        {
            if (player.Scp106Controller.IsUsingPortal) return false;

            if(player.Room.Zone == Synapse.Api.Enum.ZoneType.Pocket)
            {
                var portalpos = player.Scp106Controller.PortalPosition;
                player.Scp106Controller.PortalPosition = oldPos;
                player.Scp106Controller.UsePortal();
                MEC.Timing.CallDelayed(AnimationDuration, () => player.Scp106Controller.PortalPosition = portalpos);
            }
            else
            {
                oldPos = player.Position;
                oldPos.y -= 2;
                var portalpos = player.Scp106Controller.PortalPosition;
                player.Scp106Controller.PortalPosition = Vector3.up * -2000;
                player.Scp106Controller.UsePortal();
                MEC.Timing.CallDelayed(AnimationDuration, () => player.Scp106Controller.PortalPosition = portalpos);
            }

            return true;
        }
        #endregion

        #region Stalk
        public float stalkCanBeUsedTime = 0;

        public float createportaltime = 0;

        public void Stalk(bool check)
        {
            if (!PluginClass.Config.Stalk || player.Scp106Controller.IsUsingPortal) return;

            if (check)
            {
                var flag = Time.time - createportaltime > 2;

                createportaltime = Time.time;

                if (flag)
                    return;
            }

            if (stalkCanBeUsedTime > Time.time)
            {
                player.GiveTextHint($"Stalk can be used in {System.Math.Ceiling(stalkCanBeUsedTime - Time.time)} seconds");
                return;
            }

            var players = Server.Get.GetPlayers(x => x.RealTeam != Team.SCP && x.RoleType != RoleType.Spectator && x.RoleType != RoleType.Scp079 && x.RoleType != RoleType.None && x.Room.Zone != Synapse.Api.Enum.ZoneType.Pocket);

            if (players.Count == 0)
            {
                player.GiveTextHint("No target to Stalk was found");
                return;
            }

            var pos = players.ElementAt(UnityEngine.Random.Range(0, players.Count)).Position;
            pos.y -= 2;

            var portalpos = player.Scp106Controller.PortalPosition;
            player.Scp106Controller.PortalPosition = pos;
            player.Scp106Controller.UsePortal();
            MEC.Timing.CallDelayed(AnimationDuration, () => player.Scp106Controller.PortalPosition = portalpos);

            stalkCanBeUsedTime = Time.time + PluginClass.Config.StalkCooldown;
            Announced = false;
            return;
        }
        #endregion

        #region PocketTrap
        public void DoPocketTrapAnimation(bool sinkhole)
        {
            if (player.GodMode || player.Room.Zone == Synapse.Api.Enum.ZoneType.Pocket) return;

            if (!sinkhole && RoleType.Scp106.GetPlayers().Count > 0 && Server.Get.Players.Any(x => x.ClassManager.Scp106 != null && x.ClassManager.Scp106.goingViaThePortal))
                return;

            if (player.ClassManager.Scp106.goingViaThePortal) return;

            player.ClassManager.Scp106.goingViaThePortal = true;

            MEC.Timing.RunCoroutine(PocketTrapAnimation(sinkhole));
        }

        private IEnumerator<float> PocketTrapAnimation(bool sinkhole)
        {
            for (int i = 0; i < 50; i++)
            {
                var pos = player.Position;
                pos.y -= 0.75f;
                player.Position = pos;

                yield return MEC.Timing.WaitForOneFrame;
            }

            player.Position = Vector3.up * -1997;
            player.GiveEffect(Synapse.Api.Enum.Effect.Corroding);

            yield return MEC.Timing.WaitForSeconds(0.1f);
            player.ClassManager.Scp106.goingViaThePortal = false;
            if (sinkhole)
                player.GiveEffect(Synapse.Api.Enum.Effect.SinkHole, 0);
        }
        #endregion

        public const float AnimationDuration = 3.2f;

        public bool Announced = false;

        public void Update()
        {
            if (player.RoleType != RoleType.Scp106) return;
            if(!Announced && Time.time >= stalkCanBeUsedTime)
            {
                Announced = true;
                player.GiveTextHint("Stalk can now be used");
            }
        }
    }
}
