using Exiled.API.Features.Items;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using Mirror;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Interactables.Interobjects.DoorUtils;
using PlayerStatsSystem;

namespace FacilityManagement
{
    public class EventHandlers
    {
        public EventHandlers(FacilityManagement plugin) => this.plugin = plugin;
        public FacilityManagement plugin;
        public int LuresCount;

        public void HandleRoundStart()
        {
            LuresCount = 0;
            if (plugin.Config.WindowHealth is not null)
            {
                foreach (Window window in Window.List)
                {
                    if (plugin.Config.WindowHealth.TryGetValue(window.Type, out float WindowHealth))
                        window.Health = WindowHealth;
                }
            }
            if (plugin.Config.DoorHealth is not null)
            {
                foreach (Door door in Door.List)
                {
                    if (plugin.Config.DoorHealth.TryGetValue(door.Type, out float DoorHealth))
                    {
                        door.MaxHealth = DoorHealth;
                        door.Health = DoorHealth;
                    }
                }
            }
            if (plugin.Config.DoorDamageTypeIgnored is not null)
            {
                foreach (Door door in Door.List)
                {
                    if (plugin.Config.DoorDamageTypeIgnored.TryGetValue(door.Type, out DoorDamageType doorDamageType))
                        door.IgnoredDamageTypes = doorDamageType;
                }
            }
            if (plugin.Config.LiftMoveDuration is not null)
            {
                foreach (Exiled.API.Features.Lift lift in Exiled.API.Features.Lift.List)
                {
                    if (plugin.Config.LiftMoveDuration.TryGetValue(lift.Type, out float LiftTime))
                        lift.MovingSpeed = LiftTime;
                }
            }
            if (plugin.Config.GeneratorDuration > -1)
            {
                foreach (Generator generator in Generator.List)
                    generator.Base._unlockCooldownTime = plugin.Config.GeneratorDuration;
            }
            if (plugin.Config.Scp106LureAmount < 1)
                Object.FindObjectOfType<LureSubjectContainer>().SetState(false, true);
        }
        public void HandleWeaponShoot(ShootingEventArgs ev)
        {
            if (ev.Shooter.CurrentItem is null)
                return;
            if (plugin.Config.InfiniteAmmo is not null && plugin.Config.InfiniteAmmo.Contains(ev.Shooter.CurrentItem.Type) &&  ev.Shooter.CurrentItem is Firearm firearm)
            {
                firearm.Ammo++;
            }
        }
        public void HandleEnergyMicroHid(UsingMicroHIDEnergyEventArgs ev)
        {
            ev.Drain *= plugin.Config.EnergyMicroHid;
        }
        public void HandleEnergyRadio(UsingRadioBatteryEventArgs ev)
        {
            ev.Drain *= plugin.Config.EnergyRadio;
        }

        public void OnSpawning(SpawningEventArgs ev)
        {
            if (plugin.Config.RoleTypeHumeShield is not null && plugin.Config.RoleTypeHumeShield.TryGetValue(ev.Player.Role.Type, out AhpProccessBuild ahpProccessBuild))
            {
                ev.Player.ActiveArtificialHealthProcesses.ToList().RemoveAll(x => true);
                ev.Player.AddAhp(ahpProccessBuild.Amount, ahpProccessBuild.Amount, -ahpProccessBuild.Regen, ahpProccessBuild.Efficacy, ahpProccessBuild.Sustain, ahpProccessBuild.Regen > 0);
            }
        }

        public void OnHurting(HurtingEventArgs ev)
        {
            if (plugin.Config.RoleTypeHumeShield is not null && plugin.Config.RoleTypeHumeShield.TryGetValue(ev.Target.Role.Type, out AhpProccessBuild ahpProccessBuild))
                ev.Target.ActiveArtificialHealthProcesses.First().SustainTime = ahpProccessBuild.Sustain;
        }

        public void HandleFemurEnter(EnteringFemurBreakerEventArgs ev)
        {
            // That means the femur breaker is always open
            if (plugin.Config.Scp106LureAmount < 1)
                return;

            // Allowed team check
            if (plugin.Config.Scp106LureTeam is not null && !plugin.Config.Scp106LureTeam.Contains(ev.Player.Role.Team))
            {
                ev.IsAllowed = false;
                return;
            }
            if (Random.Range(0, 100) >= plugin.Config.Scp106ChanceOfSuccess)
                LuresCount = plugin.Config.Scp106LureAmount;
            if (++LuresCount < plugin.Config.Scp106LureAmount)
            {
                FacilityManagement.Singleton.RoundCoroutines.Add(Timing.CallDelayed(Mathf.Max(plugin.Config.Scp106LureReload, 0), () =>
                {
                    Object.FindObjectOfType<LureSubjectContainer>().NetworkallowContain = false;
                }));
            }
        }

        public void HandleContain106(ContainingEventArgs ev)
        {
            if (plugin.Config.Scp106LureAmount < 1)
                return;
            ev.IsAllowed = LuresCount > plugin.Config.Scp106LureAmount;
        }
       
        
        public void HandleWarheadDetonation()
        {
            if (!plugin.Config.WarheadCleanup)
                return;

            foreach (Pickup pickup in Map.Pickups)
            {
                if (pickup.Position.y < 500f)
                    pickup.Destroy();
            }

            foreach (Exiled.API.Features.Ragdoll ragdoll in Map.Ragdolls)
            {
                if (ragdoll.Position.y < 500f)
                    ragdoll.Delete();
            }
        }
    }
}
