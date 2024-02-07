using HarmonyLib;
using InventorySystem.Items;

using Inventory = InventorySystem.Inventory;
using Exiled.API.Features.Items;
using Exiled.API.Features;
using System;

namespace FacilityManagement.Patches
{
    [HarmonyPatch(typeof(Item), MethodType.Constructor, new Type[] { typeof(ItemBase), })]
    public static class NewItemConfig
    {
        private static void Postfix(Item __instance, ItemBase itemBase)
        {
            Log.Warn("YAmato");
            FacilityManagement.Singleton.EventHandlers.CustomItem(__instance);
        }
    }
}
