using Exiled.API.Enums;
using Exiled.API.Interfaces;
using Interactables.Interobjects.DoorUtils;
using System.Collections.Generic;
using System.ComponentModel;
using KeycardPermissions = Interactables.Interobjects.DoorUtils.KeycardPermissions;

namespace FacilityManagement
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = false;

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

        [Description("Sets the time for Lift to teleport")]
        public Dictionary<ElevatorType, float> LiftMoveDuration { get; set; } = new() 
        {
            {ElevatorType.Nuke, 8}
        };

        [Description(@"Custom intercom content. If there's no specific content, then the default client content is used.
        # Check GitHub ReadMe for more info (https://github.com/louis1706/FacilityManagement/blob/main/readme.md)")]
        public Dictionary<Intercom.State, string> CustomText { get; set; } = new()
        {
            {Intercom.State.Ready, "Ready"},
            {Intercom.State.Transmitting, "{intercom_speaker_nickname} speaking {intercom_speech_remaining_time}"},
            {Intercom.State.Restarting, "Restarting please wait for {intercom_remaining_cooldown}"},
            {Intercom.State.AdminSpeaking, "the Admin {intercom_speaker_nickname} is actually speaking"},
            {Intercom.State.Muted, "Issou you are muted"},
            {Intercom.State.Custom, "{intercom_custom_text}"},
        };
        [Description("How mush the CustomText for intercom will be refresh (empty make refresh everytick)")]
        public float? IntercomRefresh { get; set; } = null;

        [Description("If all items and ragdolls in the facility should be removed after detonation.")]
        public bool WarheadCleanup { get; set; } = true;

        [Description("How many sacrifices it takes to lure 106. Values below 1 set the recontainer to always active.")]
        public int Scp106LureAmount { get; set; } = 1;

        [Description("Probability of succes for sacrifice work before enough player was enter in the recontainer")]
        public float Scp106ChanceOfSuccess { get; set; } = 100;

        [Description("Amount of time before another sacrifice can be made.")]
        public int Scp106LureReload { get; set; } = 0;

        [Description("RoleType that can't enter the femur breaker.")]
        public List<RoleType> Scp106ContainerIgonredRoles { get; set; } = new()
        {
            RoleType.Tutorial,
            RoleType.NtfCaptain,
        };
        [Description("RoleType than do not trigger tesla.")]
        public List<RoleType> TeslaIgnoredRoles { get; set; } = new()
        {
            RoleType.Tutorial,
            RoleType.NtfCaptain,
        };

        [Description("Sets the config of Tesla.")]
        public TeslaBuild CustomTesla { get; set; } = new()
        {
            ActivationTime = 0, 
            IdleRange = 25,
            TriggerRange = 10,
            IgnoredRoles = new()
            {
                RoleType.Scp0492,
            }
        };

        [Description("Sets the health of breakable windows.")]
        public Dictionary<GlassType, GlassBuild> CustomWindows { get; set; } = new()
        {
            {
                GlassType.Scp079Trigger, 
                new GlassBuild{
                        Health = 100,
                        DisableScpDamage = false,
                } 
            },
        };

        [Description("Sets the ignored damage of breakable Door (0 will make it Destructible for everything and 17 undestructible).")]
        public Dictionary<DoorType, DoorBuild> CustomDoors { get; set; } = new()
        {
            { 
                DoorType.CheckpointEntrance,
                new DoorBuild{
                            Health = 30,
                            RequiredPermission = KeycardPermissions.ContainmentLevelThree | KeycardPermissions.Checkpoints | KeycardPermissions.ScpOverride,
                            RequireAllPermission = false,
                            DamageTypeIgnored = DoorDamageType.Grenade,
                }
            },
            { 
                DoorType.GR18Inner,
                new DoorBuild{
                            Health = 120,
                            RequiredPermission = KeycardPermissions.None,
                            RequireAllPermission = null,
                            DamageTypeIgnored = 0,
                }
            },
        };

        public Dictionary<RoleType, AhpProccessBuild> RoleTypeHumeShield { get; set; } = new()
        {
            { 
                RoleType.Scp049,
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
