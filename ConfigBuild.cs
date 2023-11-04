using Interactables.Interobjects.DoorUtils;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;
using Exiled.API.Extensions;
using System;
using Exiled.API.Features;

namespace FacilityManagement
{
    public class AhpProccessBuild
    {
        public float Amount { get; set; }
        public float Regen { get; set; }
        public float Efficacy { get; set; }
        public float Sustain { get; set; }
    }

    public class DoorBuild
    {
        public float? Health { get; set; }
        public KeycardPermissions? RequiredPermission { get; set; }
        public bool? RequireAllPermission { get; set; }
        public DoorDamageType? DamageTypeIgnored { get; set; }
    }

    public class GlassBuild
    {
        public float? Health { get; set; }
        public bool? DisableScpDamage { get; set; }
    }

    public class TeslaBuild
    {
        public List<RoleTypeId> IgnoredRoles { get; set; }
        public float? TriggerRange { get; set; }
        public float? IdleRange { get; set; }
        public float? ActivationTime { get; set; }
        public float? CooldownTime { get; set; }
    }

    public class Scp914Build
    {
        public float? KnobChangeCooldown { get; set; }
        public float? DoorCloseTime { get; set; }
        public float? ItemUpgradeTime { get; set; }
        public float? DoorOpenTime { get; set; }
        public float? ActivationCooldown { get; set; }
    }

    public class GeneratorBuild
    {
        public float? UnlockCooldown { get; set; }
        public float? LeverDelay { get; set; }
        public float? DoorPanelCooldown { get; set; }
        public float? InteractionCooldown { get; set; }
        public float? DeactivationTime { get; set; }
        public Exiled.API.Enums.KeycardPermissions? RequiredPermission { get; set; }
    }

    public class ItemBuild
    {
        public Dictionary<string, string> Custom { get; set; }

        public static object Parse(string value, Type targetType, out bool success)
        {
            success = true;
            // Handle nullable value types (e.g., int?, float?, double?)
            if (Nullable.GetUnderlyingType(targetType) != null)
            {
                // Check if the value is null or empty and return null
                if (string.IsNullOrWhiteSpace(value))
                {
                    success = false;
                    return default;
                }

                try
                {
                    return Convert.ChangeType(value, targetType);
                }
                catch (InvalidCastException)
                {
                    success = false;
                    return default;
                }
            }

            // Handle enums
            if (targetType.IsEnum)
            {
                return Enum.Parse(targetType, value);
            }

            // Handle Vector3 (assuming the format is "(x,y,z)")
            if (targetType == typeof(Vector3))
            {
                value = value.Replace('(', ' ').Replace(')', ' ');
                string[] components = value.Split(',');
                if (components.Length == 3 && float.TryParse(components[0], out float x) && float.TryParse(components[1], out float y) && float.TryParse(components[2], out float z))
                {
                    return new Vector3(x, y, z);
                }
            }

            // Handle numbers (int, float, double, etc.)
            if (targetType.IsValueType)
            {
                try
                {
                    return Convert.ChangeType(value, targetType);
                }
                catch (InvalidCastException)
                {
                    Log.Error("Invalid Number");
                }
            }
            Log.Error("End");
            success = false;
            return default;
        }

    }

    /* Wait Futur Exiled
    public class AnnimationCurveBuild
    {
        public float? AddCurve { get; set; }
        public float? MultiplyCurve { get; set; }

        public AnimationCurve ModifyCurve(AnimationCurve animationCurve)
        {
            if (AddCurve is not null)
                animationCurve.Add(AddCurve.Value);
            if (MultiplyCurve is not null)
                animationCurve.Multiply(MultiplyCurve.Value);
            return animationCurve;
        }
    }
    public class FlashbangBuild
    {
        public AnnimationCurveBuild BlindingOverDistance { get; set; }
        public AnnimationCurveBuild TurnedAwayBlindingDistance { get; set; }
        public AnnimationCurveBuild DeafenDurationOverDistance { get; set; }
        public AnnimationCurveBuild TurnedAwayDeafenDurationOverDistance { get; set; }
        public float? DurfaceZoneDistanceIntensifier { get; set; }
        public float? AdditionalBlurDuration { get; set; }
        public float? MinimalEffectDuration { get; set; }
        public float? BlindTime { get; set; }
    }

    public class FragGrenadeBuild
    {
        public AnnimationCurveBuild BlindingOverDistance { get; set; }
        public AnnimationCurveBuild TurnedAwayBlindingDistance { get; set; }
        public AnnimationCurveBuild DeafenDurationOverDistance { get; set; }
        public AnnimationCurveBuild TurnedAwayDeafenDurationOverDistance { get; set; }
        public float? DurfaceZoneDistanceIntensifier { get; set; }
        public float? AdditionalBlurDuration { get; set; }
        public float? MinimalEffectDuration { get; set; }
        public float? BlindTime { get; set; }
    }

    public class Scp244Build
    {
        public AnnimationCurveBuild DamageOverTemperature { get; set; }
        public AnnimationCurveBuild GrowSpeedOverLifetime { get; set; }
        public float? MaxExitTemp { get; set; }
        public float? TemperatureDrop { get; set; }
        public float? MinimalEffectDuration { get; set; }
        public float? BlindTime { get; set; }
        public float? Scp244Health { get; set; }
        public float? DeployedPickupTime { get; set; }
    }
    /*
    public class Scp018Build
    {
        public AnnimationCurveBuild DamageOverVelocity { get; set; }
        public float? MaximumVelocity { get; set; }
        public float? OnBounceVelocityAddition { get; set; }
        public float? ActivationVelocitySqr { get; set; }
        public float? DoorDamageMultiplier { get; set; }
        public float? ScpDamageMultiplier { get; set; }
        public float? FriendlyFireTime { get; set; }
        public float? BounceHitregRadius { get; set; }
    }

    public class ExplosionGrenadeBuild
    {
        public AnnimationCurveBuild PlayerDamageOverDistance { get; set; }
        public AnnimationCurveBuild EffectDurationOverDistance { get; set; }
        public AnnimationCurveBuild DoorDamageOverDistance { get; set; }
        public float? ScpDamageMultiplier { get; set; }
        public float? MaxRadius { get; set; }
        public float? BurnedDuration { get; set; }
        public float? DeafenedDuration { get; set; }
        public float? ConcussedDuration { get; set; }
        public float? MinimalDuration { get; set; }
        public float? BounceHitregRadius { get; set; }
    }

    public class RegenerationBuild
    {
        public AnnimationCurveBuild Scp500HealProgress { get; set; }
        public AnnimationCurveBuild PainkillersHealProgress { get; set; }
    }
    public class Scp939Build
    {
        public AnnimationCurveBuild StaminaRegeneration { get; set; }
    }*/
}
