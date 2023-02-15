using Interactables.Interobjects.DoorUtils;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

}
