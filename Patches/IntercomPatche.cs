using HarmonyLib;
using System;
using Exiled.API.Features;
using UnityEngine;

namespace FacilityManagement.Patches
{
    public class IntercomUpdateTextPatche
    {
        [HarmonyPatch(typeof(Intercom), nameof(Intercom.IntercomState), MethodType.Setter)]
        public static class CommandIntercomTextSetterFix
        {
            public static bool Prefix(Intercom __instance, ref Intercom.State value) 
            {
                __instance.Network_state = SetContent(__instance, value);
                return false; 
            }
        }
        internal static Intercom.State SetContent(Intercom intercom, Intercom.State state)
        {
            if (FacilityManagement.Singleton.CustomText is not null)
                state = Intercom.State.Custom;
            if (FacilityManagement.Singleton.Config.CustomText is null || !FacilityManagement.Singleton.Config.CustomText.TryGetValue(state, out string content))
                return state;
            if (!ServerConsole.singleton.NameFormatter.TryProcessExpression(content, "FacilityManagement", out string result))
            {
                Log.Error(result);
                return state;
            }
            intercom.Network_intercomText = result;
            return Intercom.State.Custom;
        }
    }
}

