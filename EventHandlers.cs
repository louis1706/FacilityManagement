﻿using Exiled.API.Features.Items;
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
using Exiled.Events.EventArgs.Scp106;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Player;

using Tesla = Exiled.API.Features.TeslaGate;

namespace FacilityManagement
{
    public class EventHandlers
    {
        public EventHandlers(FacilityManagement plugin) => this.plugin = plugin;
        public FacilityManagement plugin;
        public int LuresCount;

        public void OnRoundStarted()
        {
            LuresCount = 0;
            if (plugin.Config.CustomTesla is not null)
            {
                foreach (Tesla tesla in Tesla.List)
                {
                    if (plugin.Config.CustomTesla.CooldownTime is not null)
                        tesla.CooldownTime = plugin.Config.CustomTesla.CooldownTime.Value;
                    if (plugin.Config.CustomTesla.IdleRange is not null)
                        tesla.IdleRange = plugin.Config.CustomTesla.IdleRange.Value;
                    if (plugin.Config.CustomTesla.TriggerRange is not null)
                        tesla.TriggerRange = plugin.Config.CustomTesla.TriggerRange.Value;
                    if (plugin.Config.CustomTesla.ActivationTime is not null)
                        tesla.ActivationTime = plugin.Config.CustomTesla.ActivationTime.Value;
                }

                if (plugin.Config.CustomTesla.IgnoredRoles is not null)
                    Tesla.IgnoredRoles = plugin.Config.CustomTesla.IgnoredRoles;
            }
            if (plugin.Config.CustomWindows is not null)
            {
                foreach (Window window in Window.List)
                {
                    if (plugin.Config.CustomWindows.TryGetValue(window.Type, out GlassBuild glassBuild))
                    {
                        if (glassBuild.Health is not null)
                            window.Health = glassBuild.Health.Value;
                        if (glassBuild.DisableScpDamage is not null)
                            window.DisableScpDamage = glassBuild.DisableScpDamage.Value;
                    }
                }
            }
            if (plugin.Config.CustomDoors is not null)
            {
                foreach (Door door in Door.List)
                {
                    if (plugin.Config.CustomDoors.TryGetValue(door.Type,out DoorBuild doorBuild))
                    {
                        if (doorBuild.Health is not null)
                            door.Health = doorBuild.Health.Value;
                        if (doorBuild.DamageTypeIgnored is not null)
                            door.IgnoredDamageTypes = doorBuild.DamageTypeIgnored.Value;
                        if (doorBuild.RequiredPermission is not null)
                            door.RequiredPermissions.RequiredPermissions = doorBuild.RequiredPermission.Value;
                        if (doorBuild.RequireAllPermission is not null)
                            door.RequiredPermissions.RequireAll = doorBuild.RequireAllPermission.Value;
                    }
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
            if (plugin.Config.Scp106ContainerIgonredRoles is not null)
            {
                Scp106Container.IgnoredRoles = plugin.Config.Scp106ContainerIgonredRoles;
            }
            if (plugin.Config.TeslaIgnoredRoles is not null)
            {
                Exiled.API.Features.TeslaGate.IgnoredRoles = plugin.Config.Scp106ContainerIgonredRoles;
            }

            if (plugin.Config.Scp106LureAmount < 1)
                Object.FindObjectOfType<LureSubjectContainer>().SetState(false, true);
        }
        public void OnShooting(ShootingEventArgs ev)
        {
            if (ev.Player.CurrentItem is null)
                return;
            if (plugin.Config.InfiniteAmmo is not null && plugin.Config.InfiniteAmmo.Contains(ev.Player.CurrentItem.Type) &&  ev.Player.CurrentItem is Firearm firearm)
            {
                firearm.Ammo++;
            }
        }
        public void OnUsingMicroHIDEnergy(UsingMicroHIDEnergyEventArgs ev)
        {
            ev.Drain *= plugin.Config.EnergyMicroHid;
        }
        public void OnUsingRadioBattery(UsingRadioBatteryEventArgs ev)
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
        public void OnEnteringFemurBreaker(EnteringFemurBreakerEventArgs _)
        {
            // That means the femur breaker is always open
            if (plugin.Config.Scp106LureAmount < 1)
                return;

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

        public void OnContaining(ContainingEventArgs ev)
        {
            if (plugin.Config.Scp106LureAmount < 1)
                return;
            ev.IsAllowed = LuresCount > plugin.Config.Scp106LureAmount;
        }
       
        
        public void OnDetonated()
        {
            if (!plugin.Config.WarheadCleanup)
                return;

            foreach (Pickup pickup in Pickup.List)
            {
                try
                {
                    if (pickup.Position.y < 500f)
                        pickup.Destroy();
                    string s = $"Pickup On detonated FacilityManagement : Pickup Is Null{pickup is null} ";
                    if (pickup is not null)
                    {
                        s += $"Pickup::Base ? " + pickup.Base is null;
                        if (pickup.Base is not null)
                            s += $"Pickup::Base::Rb ? " + pickup.Base.Rb is null;
                    }
                    Log.Error(s + "  \n ");

                }
                catch (System.Exception ex)
                {
                    Log.Error(ex);
                }
            }

            foreach (Exiled.API.Features.Ragdoll ragdoll in Map.Ragdolls)
            {
                if (ragdoll.Position.y < 500f)
                    ragdoll.Delete();
            }
        }
    }
}
