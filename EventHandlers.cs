using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using InventorySystem;
using InventorySystem.Configs;
using InventorySystem.Items;
using PlayerRoles.Subroutines;
using PlayerStatsSystem;
using Scp914;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BreakableDoor = Exiled.API.Features.Doors.BreakableDoor;
using Tesla = Exiled.API.Features.TeslaGate;

namespace FacilityManagement
{
    public class EventHandlers
    {
        public EventHandlers(FacilityManagement plugin) => this.plugin = plugin;
        public FacilityManagement plugin;

        public void OnWaitingForPlayers()
        {
            if (plugin.Config.CustomTesla is not null)
                CustomTesla();
            if (plugin.Config.CustomScp914 is not null)
                Custom914();
            if (plugin.Config.CustomWindows is not null)
                CustomWindow();
            if (plugin.Config.CustomDoors is not null)
                CustomDoor();
            if (plugin.Config.CustomGenerator is not null)
                CustomGenerator();

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
            if (plugin.Config.LiftMoveDuration is not null)
            {
                foreach (var elevator in Lift.List)
                    if (plugin.Config.LiftMoveDuration.TryGetValue(elevator.Type, out float value))
                        elevator.AnimationTime = value;
            }
        }
        public void OnChangingAmmo(ChangingAmmoEventArgs ev)
        {
            if (plugin.Config.InfiniteAmmo.Contains(ev.Firearm.Type))
            {
                ev.IsAllowed = false;
            }
        }
        public void OnUsingMicroHIDEnergy(UsingMicroHIDEnergyEventArgs ev) => ev.Drain *= plugin.Config.EnergyMicroHid;
        public void OnUsingRadioBattery(UsingRadioBatteryEventArgs ev) => ev.Drain *= plugin.Config.EnergyRadio;
        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (plugin.Config.RoleTypeHumeShield is not null && plugin.Config.RoleTypeHumeShield.TryGetValue(ev.Player.Role.Type, out AhpProccessBuild ahpProccessBuild))
            {
                ev.Player.ReferenceHub.playerStats.GetModule<AhpStat>()._activeProcesses.Clear();
                ev.Player.AddAhp(ahpProccessBuild.Amount, ahpProccessBuild.Amount, -ahpProccessBuild.Regen, ahpProccessBuild.Efficacy, ahpProccessBuild.Sustain, ahpProccessBuild.Regen > 0);
            }
            if (plugin.Config.CustomRole is not null)
                CustomRole(ev.Player);
        }

        public void OnHurting(HurtingEventArgs ev)
        {
            if (plugin.Config.RoleTypeHumeShield.TryGetValue(ev.Player.Role.Type, out AhpProccessBuild ahpProccessBuild) && ahpProccessBuild.Regen > 0)
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
                string Debug = "[CustomTesla]\n";
                {
                    Tesla tesla = Tesla.List.First();
                    Debug += $"CooldownTime: {tesla.CooldownTime} => {plugin.Config.CustomTesla.CooldownTime}\n";
                    Debug += $"IdleRange: {tesla.IdleRange} => {plugin.Config.CustomTesla.IdleRange}\n\n";
                    Debug += $"TriggerRange: {tesla.TriggerRange} => {plugin.Config.CustomTesla.TriggerRange}\n\n";
                    Debug += $"ActivationTime: {tesla.ActivationTime} => {plugin.Config.CustomTesla.ActivationTime}\n\n";
                    Debug += $"IgnoredRoles: {string.Join(",", Tesla.IgnoredRoles)} => {(plugin.Config.CustomTesla.IgnoredRoles is null ? "Null" : string.Join(",", plugin.Config.CustomTesla.IgnoredRoles))}\n\n";
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

        public void Custom914()
        {
            Scp914Controller scp914 = Exiled.API.Features.Scp914.Scp914Controller;
            Scp914Build scp914Build = plugin.Config.CustomScp914;
            if (FacilityManagement.Singleton.Config.Debug)
            {
                string Debug = "[Custom914]\n";
                {
                    Debug += $"KnobChangeCooldown: {scp914._knobChangeCooldown} => {scp914Build.KnobChangeCooldown}\n";
                    Debug += $"DoorOpenTime: {scp914._doorOpenTime} => {scp914Build.DoorOpenTime}\n";
                    Debug += $"ItemUpgradeTime: {scp914._itemUpgradeTime} => {scp914Build.ItemUpgradeTime}\n";
                    Debug += $"DoorCloseTime: {scp914._doorCloseTime} => {scp914Build.DoorCloseTime}\n";
                    Debug += $"TotalSequenceTime: {scp914._totalSequenceTime} => {scp914Build.ActivationCooldown}\n";
                }
                Log.Debug(Debug);
            }

            if (scp914Build.KnobChangeCooldown is not null)
                scp914._knobChangeCooldown = scp914Build.KnobChangeCooldown.Value;
            if (scp914Build.DoorOpenTime is not null)
                scp914._doorOpenTime = scp914Build.DoorOpenTime.Value;
            if (scp914Build.ItemUpgradeTime is not null)
                scp914._itemUpgradeTime = scp914Build.ItemUpgradeTime.Value;
            if (scp914Build.DoorCloseTime is not null)
                scp914._doorCloseTime = scp914Build.DoorCloseTime.Value;
            if (scp914Build.ActivationCooldown is not null)
                scp914._totalSequenceTime = scp914Build.ActivationCooldown.Value;

        }
        public void CustomWindow()
        {
            if (FacilityManagement.Singleton.Config.Debug)
            {
                string Debug = "[CustomWindow]\n";
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
        public void CustomGenerator()
        {
            GeneratorBuild generator914Build = plugin.Config.CustomGenerator;

            if (FacilityManagement.Singleton.Config.Debug)
            {
                Generator generator = Generator.List.First();
                string Debug = "[CustomGenerator]\n";
                Debug += $"UnlockCooldown: {generator.UnlockCooldown} => {generator914Build.UnlockCooldown}\n";
                Debug += $"LeverDelay: {generator.LeverDelay} => {generator914Build.LeverDelay}\n";
                Debug += $"TogglePanelCooldown: {generator.TogglePanelCooldown} => {generator914Build.DoorPanelCooldown}\n";
                Debug += $"InteractionCooldown: {generator.InteractionCooldown} => {generator914Build.InteractionCooldown}\n";
                Debug += $"DeactivationTime: {generator.DeactivationTime} => {generator914Build.DeactivationTime}\n";
                Debug += $"KeycardPermissions: {generator.KeycardPermissions} => {generator914Build.RequiredPermission}\n";
                Log.Debug(Debug);
            }
            foreach (Generator generator in Generator.List)
            {
                if (generator914Build.UnlockCooldown is not null)
                    generator.UnlockCooldown = generator914Build.UnlockCooldown.Value;
                if (generator914Build.RequiredPermission is not null)
                    generator.KeycardPermissions = generator914Build.RequiredPermission.Value;
                if (generator914Build.LeverDelay is not null)
                    generator.LeverDelay = generator914Build.LeverDelay.Value;
                if (generator914Build.DoorPanelCooldown is not null)
                    generator.TogglePanelCooldown = generator914Build.DoorPanelCooldown.Value;
                if (generator914Build.InteractionCooldown is not null)
                    generator.InteractionCooldown = generator914Build.InteractionCooldown.Value;
                if (generator914Build.DeactivationTime is not null)
                    generator.DeactivationTime = generator914Build.DeactivationTime.Value;
            }
        }
        public void CustomItem(Item newItem)
        {
            if (newItem is null || !plugin.Config.CustomItem.TryGetValue(newItem.Type, out ItemBuild itemBuild))
                return;
            foreach (KeyValuePair<string, string> e in itemBuild.Custom)
            {
                if (plugin.Config.Debug)
                    Log.Debug($"ItemType {newItem.Type} Key '{e.Key}' or Value '{e.Value}'");
                try
                {
                    PropertyInfo propertyInfo = newItem.GetType().GetProperty(e.Key);

                    if (propertyInfo != null)
                    {

                        object value = ItemBuild.Parse(e.Value, propertyInfo.PropertyType, out bool success);
                        if (success)
                            propertyInfo.SetValue(newItem, value);
                        else
                            Log.Error("invalid cast");
                    }
                    else
                        Log.Error("Property not found: " + e.Key);
                }
                catch (Exception ex)
                {
                    Log.Error($"CustomItem {newItem.Type} invalid Key '{e.Key}' or Value '{e.Value}'\n{ex}");
                }
            }
        }

        public void CustomRole(Player player)
        {
            if (player is null || !plugin.Config.CustomRole.TryGetValue(player.Role.Type, out RoleBuild roleBuild))
                return;
            foreach (KeyValuePair<string, string> e in roleBuild.Custom)
            {
                if (plugin.Config.Debug)
                    Log.Debug($"RoleType {player.Role.Type} Key '{e.Key}' or Value '{e.Value}'");
                try
                {
                    PropertyInfo propertyInfo = player.Role.GetType().GetProperty(e.Key);

                    if (propertyInfo != null)
                    {
                        if (typeof(StandardSubroutine<>).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            Log.Error($"This will be supported in an imaginary futur {e.Key}");
                            continue;
                        }
                        object value = ItemBuild.Parse(e.Value, propertyInfo.PropertyType, out bool success);
                        if (success)
                            propertyInfo.SetValue(player.Role, value);
                        else
                            Log.Error("invalid cast");
                    }
                    else
                        Log.Error("Property not found: " + e.Key);
                }
                catch (Exception ex)
                {
                    Log.Error($"CustomRole {player.Role.Type} invalid Key '{e.Key}' or Value '{e.Value}'\n{ex}");
                }
            }
        }
        public void CustomDoor()
        {
            foreach (Door door in Door.List)
            {
                CustomDoorSet(door, door.Type);
            }
        }
        public void CustomDoorSet(Door door, DoorType type)
        {
            if (plugin.Config.CustomDoors.TryGetValue(type, out DoorBuild doorBuild))
            {
                BreakableDoor breakabledoor = door.As<BreakableDoor>();

                if (FacilityManagement.Singleton.Config.Debug)
                {
                    string Debug = $"[CustomDoor] : {type}\n";
                    Debug += $"Health: {(breakabledoor is null ? "Nan" : breakabledoor.Health)} => {doorBuild.Health.Value}\n";
                    Debug += $"IgnoredDamageTypes: {(breakabledoor is null ? "Nan" : breakabledoor.IgnoredDamage)} => {doorBuild.DamageTypeIgnored}\n";
                    Debug += $"RequiredPermissions: {door.RequiredPermissions.RequiredPermissions} => {doorBuild.RequiredPermission}\n";
                    Debug += $"RequireAllPermission: {door.RequiredPermissions.RequireAll} => {doorBuild.RequireAllPermission}\n";
                    Log.Debug(Debug);
                }

                if (doorBuild.Health is not null && breakabledoor is not null)
                    breakabledoor.Health = doorBuild.Health.Value;
                if (doorBuild.DamageTypeIgnored is not null && breakabledoor is not null)
                    breakabledoor.IgnoredDamage = doorBuild.DamageTypeIgnored.Value;
                if (doorBuild.RequiredPermission is not null)
                    door.RequiredPermissions.RequiredPermissions = doorBuild.RequiredPermission.Value;
                if (doorBuild.RequireAllPermission is not null)
                    door.RequiredPermissions.RequireAll = doorBuild.RequireAllPermission.Value;
            }
        }
    }
}
