using HarmonyLib;

namespace FacilityManagement.Patches
{
    [HarmonyPatch(typeof(Exiled.API.Features.Intercom), nameof(Exiled.API.Features.Intercom.DisplayText), MethodType.Setter)]
    public static class CommandIntercomTextSetterFix
    {
        public static void Prefix(ref string value) => FacilityManagement.Singleton.CustomText = value;
    }

    [HarmonyPatch(typeof(Exiled.API.Features.Intercom), nameof(Exiled.API.Features.Intercom.DisplayText), MethodType.Getter)]
    public static class CommandIntercomTextGetterFix
    {
        public static void Prefix(ref string __result) => __result = FacilityManagement.Singleton.CustomText;
    }
}