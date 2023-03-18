﻿using Exiled.API.Enums;
using Exiled.API.Interfaces;
using Exiled.Events.Handlers;
using Interactables.Interobjects.DoorUtils;
using PlayerRoles;
using PlayerRoles.Voice;
using System.Collections.Generic;
using System.ComponentModel;
using KeycardPermissions = Interactables.Interobjects.DoorUtils.KeycardPermissions;

namespace FacilityManagement
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = false;
        public bool Debug { get; set; } = false;

        [Description("Make infinite ammo for weapon.")]
        public List<ItemType> InfiniteAmmo { get; set; } = new()
        {
            ItemType.ParticleDisruptor,
        };

        [Description("Make infinite ammo for weapon.")]
        public float EnergyMicroHid { get; set; } = 1f;
        [Description("Make infinite ammo for weapon.")]
        public float EnergyRadio { get; set; } = 1f;

        [Description("Time for generator to be Activated after being enable (-1 disable the config)")]
        public float GeneratorDuration { get; set; } = -1;

        [Description("Sets the Standard ammo limits for the player")]
        public Dictionary<AmmoType, ushort> StandardAmmoLimits { get; set; } = new()
        {
            {AmmoType.Nato9, 30},
            {AmmoType.Nato556, 40},
            {AmmoType.Nato762, 40},
            {AmmoType.Ammo44Cal, 18},
            {AmmoType.Ammo12Gauge, 14},
        };
        [Description("Sets standard category limits")]
        public Dictionary<ItemCategory, sbyte> StandardCategoryLimits { get; set; } = new()
        {
            {ItemCategory.Armor, -1},
            {ItemCategory.Grenade, 2},
            {ItemCategory.Keycard, 3},
            {ItemCategory.Medical, 3},
            {ItemCategory.MicroHID, -1},
            {ItemCategory.Radio, -1},
            {ItemCategory.SCPItem, 3},
            {ItemCategory.Firearm, 1},
        };

        [Description(@"Custom intercom content. If there's no specific content, then the default client content is used.
        # Check GitHub ReadMe for more info (https://github.com/louis1706/FacilityManagement/blob/main/readme.md)")]
        public Dictionary<IntercomDisplay.IcomText, string> CustomText { get; set; } = new()
        {
            {IntercomDisplay.IcomText.Ready, "Ready"},
            {IntercomDisplay.IcomText.Transmitting, "{intercom_speaker_nickname} speaking {intercom_speech_remaining_time}"},
            {IntercomDisplay.IcomText.TrasmittingBypass, "{intercom_speaker_nickname} speaking {intercom_speech_remaining_time}"},
            {IntercomDisplay.IcomText.Restarting, "Restarting please wait for {intercom_remaining_cooldown}"},
            {IntercomDisplay.IcomText.AdminUsing, "the Admin {intercom_speaker_nickname} is actually speaking"},
            {IntercomDisplay.IcomText.Muted, "Issou you are muted"},
            {IntercomDisplay.IcomText.Wait, "Wait"},
            {IntercomDisplay.IcomText.Unknown, "Unknown"},
        };
        [Description("How mush the CustomText for intercom will be refresh (empty make refresh everytick)")]
        public float? IntercomRefresh { get; set; } = null;

        [Description("If all items and ragdolls in the facility should be removed after detonation.")]
        public bool WarheadCleanup { get; set; } = true;
        [Description("Sets the config of Tesla.")]
        public TeslaBuild CustomTesla { get; set; } = new()
        {
            ActivationTime = 0.75f, 
            IdleRange = 6.55f,
            TriggerRange = 5.1f,
            IgnoredRoles = new()
        };
        [Description("Sets the health of breakable windows.")]
        public Dictionary<GlassType, GlassBuild> CustomWindows { get; set; } = new()
        {
            {
                GlassType.Scp079Trigger, 
                new GlassBuild{
                        Health = 5,
                        DisableScpDamage = true,
                } 
            },
        };

        [Description("Sets the ignored damage of breakable Door (0 will make it Destructible for everything and -1 undestructible).")]
        public Dictionary<DoorType, DoorBuild> CustomDoors { get; set; } = new()
        {
            { 
                DoorType.CheckpointEzHczA,
                new DoorBuild{
                            Health = 30,
                            RequiredPermission = KeycardPermissions.Checkpoints | KeycardPermissions.ScpOverride,
                            RequireAllPermission = false,
                            DamageTypeIgnored = DoorDamageType.Grenade | DoorDamageType.Weapon | DoorDamageType.Scp096,
                }
            },
            {
                DoorType.CheckpointEzHczB,
                new DoorBuild{
                            Health = 30,
                            RequiredPermission = KeycardPermissions.Checkpoints | KeycardPermissions.ScpOverride,
                            RequireAllPermission = false,
                            DamageTypeIgnored = DoorDamageType.Grenade | DoorDamageType.Weapon | DoorDamageType.Scp096,
                }
            },
            { 
                DoorType.GR18Inner,
                new DoorBuild{
                            Health = 150,
                            RequiredPermission = KeycardPermissions.None,
                            RequireAllPermission = null,
                            DamageTypeIgnored = 0,
                }
            },
        };

        public Dictionary<RoleTypeId, AhpProccessBuild> RoleTypeHumeShield { get; set; } = new()
        {
            {
                RoleTypeId.Tutorial,
                new AhpProccessBuild{
                    Amount = 60,
                    Regen = 1.5f,
                    Efficacy = 1,
                    Sustain = 5,
                }
            },
        };

    }
}
