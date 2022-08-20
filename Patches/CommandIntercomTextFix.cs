using CommandSystem;
using CommandSystem.Commands.RemoteAdmin.MutingAndIntercom;
using HarmonyLib;
using Mirror;
using Mirror.LiteNetLib4Mirror;
using NorthwoodLib.Pools;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacilityManagement.Patches
{
    //[HarmonyPatch(typeof(Intercom), nameof(Intercom.CustomContent), MethodType.Setter)]
    public static class IntercomTextCommandPatches
    {
        public static void Prefix(/*Intercom __instance,*/ ref string value) => FacilityManagement.Singleton.CustomText = value;
    }
}
