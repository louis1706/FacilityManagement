using HarmonyLib;
using System.Collections.Generic;
using Utils;
using System.Reflection;
using InventorySystem.Items;
using System.Reflection.Emit;
using Exiled.API.Features.Pools;

using static HarmonyLib.AccessTools;

using Inventory = InventorySystem.Inventory;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using InventorySystem.Items.Pickups;
using Exiled.API.Features.Items;
using InventorySystem;
using System;

namespace FacilityManagement.Patches
{
    [HarmonyPatch(typeof(Inventory), nameof(Inventory.CreateItemInstance))]
    public static class NewItemConfig
    {
        private static void Postfix(ItemIdentifier identifier, bool updateViewmodel, ref ItemBase __result)
        {
            Item item = Item.Get(__result);
            FacilityManagement.Singleton.EventHandlers.CustomItem(item);
        }
    }
}
