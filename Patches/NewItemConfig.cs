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
    [HarmonyPatch(typeof(InventoryExtensions), nameof(InventoryExtensions.ServerAddItem))]
    public static class NewItemConfig
    {
        private static IEnumerable<CodeInstruction> Transpiler(
            IEnumerable<CodeInstruction> instructions,
            ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Pool.Get(instructions);

            const int offset = -2;
            int index = newInstructions.FindIndex(
                i =>
                    (i.opcode == OpCodes.Callvirt) &&
                    ((MethodInfo)i.operand == Method(typeof(ItemBase), nameof(ItemBase.OnAdded)))) + offset;

            // AddItem(Player.Get(inv._hub), itemInstance)
            newInstructions.InsertRange(
                index,
                new CodeInstruction[]
                {
                    // itemInstance
                    new(OpCodes.Ldloc_1),

                    // pickup
                    new(OpCodes.Ldarg_3),

                    // AddItem(player, itemInstance, pickup)
                    new(OpCodes.Call, Method(typeof(NewItemConfig), nameof(AddItem))),
                });


            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Pool.Return(newInstructions);
        }
        private static void AddItem(ItemBase itemBase, ItemPickupBase itemPickupBase)
        {
            Item item = Item.Get(itemBase);
            Pickup pickup = Pickup.Get(itemPickupBase);

            if (pickup is null)
                FacilityManagement.Singleton.EventHandlers.CustomItem(item);
        }
    }
    [HarmonyPatch]
    public static class NewItemConfig2
    {
        public static MethodInfo TargetMethod() => typeof(Pickup).GetMethod("InitializeProperties", (BindingFlags)(-1));
        public static MethodInfo IdcOfProtectionRule { get; } = typeof(Pickup).GetMethod("ReadItemInfo", (BindingFlags)(-1));
        public static void Prefix(Pickup __instance, ItemBase itemBase)
        {
            Item item = Item.Get(itemBase);
            FacilityManagement.Singleton.EventHandlers.CustomItem(item);
            IdcOfProtectionRule.Invoke(__instance, new object[] { item });
        }
    }
}
