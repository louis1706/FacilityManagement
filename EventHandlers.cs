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
using Tesla = Exiled.API.Features.TeslaGate;
using Interactables.Interobjects;
using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Pools;
using InventorySystem.Configs;
using Exiled.API.Extensions;

namespace FacilityManagement
{
    public class EventHandlers
    {
        public EventHandlers(FacilityManagement plugin) => this.plugin = plugin;
        public FacilityManagement plugin;
        public int LuresCount;

        public void OnWaitingForPlayers()
        {
            LuresCount = 0;
            if (plugin.Config.CustomTesla is not null)
                CustomTesla();
            if (plugin.Config.CustomWindows is not null)
                CustomWindow();
            if (plugin.Config.CustomDoors is not null)
                CustomDoor();
            if (plugin.Config.StandardAmmoLimits is not null)
            {
                InventoryLimits.StandardAmmoLimits.Clear();
                foreach (KeyValuePair<AmmoType, ushort> AmmoLimit in plugin.Config.StandardAmmoLimits)
                    InventoryLimits.StandardAmmoLimits.Add(AmmoLimit.Key.GetItemType(), AmmoLimit.Value);
            }
            if (plugin.Config.StandardCategoryLimits is not null)
            {
                InventoryLimits.StandardCategoryLimits.Clear();
                foreach (KeyValuePair<ItemCategory, sbyte> AmmoLimit in plugin.Config.StandardCategoryLimits)
                    InventoryLimits.StandardCategoryLimits.Add(AmmoLimit.Key, AmmoLimit.Value);
            }
            if (plugin.Config.GeneratorDuration > -1)
            {
                foreach (Generator generator in Generator.List)
                    generator.UnlockCooldown = plugin.Config.GeneratorDuration;
            }
        }
        public void OnShooting(ShootingEventArgs ev)
        {
            if (ev.Player.CurrentItem is null)
                return;
            if (plugin.Config.InfiniteAmmo.Contains(ev.Player.CurrentItem.Type) &&  ev.Player.CurrentItem is Firearm firearm)
            {
                firearm.Ammo++;
            }
        }
        public void OnUsingMicroHIDEnergy(UsingMicroHIDEnergyEventArgs ev) => ev.Drain *= plugin.Config.EnergyMicroHid;
        public void OnUsingRadioBattery(UsingRadioBatteryEventArgs ev) => ev.Drain *= plugin.Config.EnergyRadio;
        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (plugin.Config.RoleTypeHumeShield.TryGetValue(ev.Player.Role.Type, out AhpProccessBuild ahpProccessBuild))
            {
                ev.Player.ReferenceHub.playerStats.GetModule<AhpStat>()._activeProcesses.Clear();
                ev.Player.AddAhp(ahpProccessBuild.Amount, ahpProccessBuild.Amount, -ahpProccessBuild.Regen, ahpProccessBuild.Efficacy, ahpProccessBuild.Sustain, ahpProccessBuild.Regen > 0);
            }
        }

        public void OnHurting(HurtingEventArgs ev)
        {
            if (plugin.Config.RoleTypeHumeShield.TryGetValue(ev.Player.Role.Type, out AhpProccessBuild ahpProccessBuild))
                ev.Player.ActiveArtificialHealthProcesses.First().SustainTime = ahpProccessBuild.Sustain;
        }       
        
        public void OnDetonated()
        {
            foreach (Pickup pickup in Pickup.List.ToList())
            {
                if (pickup.Position.y < 500f)
                    pickup.Destroy();
            }
            foreach (Ragdoll ragdoll in Ragdoll.List.ToList())
            {
                if (ragdoll.Position.y < 500f)
                    ragdoll.Destroy();
            }
        }

        public void CustomTesla()
        {
            if (FacilityManagement.Singleton.Config.Debug)
            {
                string Debug = "[CustomTesla]";
                {
                    Tesla tesla = Tesla.List.First();
                    Debug += $"CooldownTime: {tesla.CooldownTime} => {plugin.Config.CustomTesla.CooldownTime}\n";
                    Debug += $"IdleRange: {tesla.IdleRange} => {plugin.Config.CustomTesla.IdleRange}\n\n";
                    Debug += $"TriggerRange: {tesla.TriggerRange} => {plugin.Config.CustomTesla.TriggerRange}\n\n";
                    Debug += $"ActivationTime: {tesla.ActivationTime} => {plugin.Config.CustomTesla.ActivationTime}\n\n";
                    Debug += $"IgnoredRoles: {string.Join(",", Tesla.IgnoredRoles)} => {string.Join(",", plugin.Config.CustomTesla.IgnoredRoles)}\n\n";
                }
                Log.Debug(Debug);
            }
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

        public void CustomWindow()
        {
            if (FacilityManagement.Singleton.Config.Debug)
            {
                string Debug = "[CustomWindow]";
                foreach (Window window in Window.List)
                {
                    if (plugin.Config.CustomWindows.TryGetValue(window.Type, out GlassBuild glassBuild))
                    {
                        Debug += $"Type: {window.Type}\n";
                        Debug += $"Health: {window.Health} => {glassBuild.Health}\n";
                        Debug += $"DisableScpDamage: {window.DisableScpDamage} => {glassBuild.DisableScpDamage}\n\n";

                        if (glassBuild.Health is not null)
                            window.Health = glassBuild.Health.Value;
                        if (glassBuild.DisableScpDamage is not null)
                            window.DisableScpDamage = glassBuild.DisableScpDamage.Value;
                    }
                }
                Log.Debug(Debug);
                return;
            }
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

        public void CustomDoor()
        {
            foreach (Door door in Door.List)
            {
                if (door.Base is CheckpointDoor checkpoint)
                {
                    foreach (DoorVariant checpointdoor in checkpoint._subDoors)
                    {
                        Log.Info(Door.Get(checpointdoor));
                        CustomDoorSet(Door.Get(checpointdoor), door.Type);
                    }
                    continue;
                }
                CustomDoorSet(door, door.Type);
            }
        }
        public void CustomDoorSet(Door door, DoorType type)
        {
            if (plugin.Config.CustomDoors.TryGetValue(type, out DoorBuild doorBuild))
            {
                if (FacilityManagement.Singleton.Config.Debug)
                {
                    string Debug = $"[CustomDoor] : {type}\n";
                    Debug += $"Health: {door.Health} => {doorBuild.Health.Value}\n";
                    Debug += $"IgnoredDamageTypes: {door.IgnoredDamageTypes} => {doorBuild.DamageTypeIgnored}\n\n";
                    Debug += $"RequiredPermissions: {door.RequiredPermissions.RequiredPermissions} => {doorBuild.RequiredPermission}\n\n";
                    Debug += $"RequireAllPermission: {door.RequiredPermissions.RequireAll} => {doorBuild.RequireAllPermission}\n\n";
                    Log.Debug(Debug);
                    return;
                }

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
}
