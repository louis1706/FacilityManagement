using Exiled.Events.EventArgs;
using HarmonyLib;
using LightContainmentZoneDecontamination;
using NorthwoodLib.Pools;
using System.Collections.Generic;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

namespace FacilityManagement.Patches.IntercomText
{
#pragma warning disable IDE0060 // Supprimer le paramètre inutilisé

    [HarmonyPatch(typeof(Exiled.API.Features.Intercom), nameof(Exiled.API.Features.Intercom.DisplayText), MethodType.Setter)]
    public static class CommandIntercomTextSetterFix
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent();

            newInstructions.AddRange(new CodeInstruction[]
            {
                new(OpCodes.Ldsfld, Field(typeof(FacilityManagement),nameof(FacilityManagement.Singleton))),
                new(OpCodes.Ldarg_0),
                new(OpCodes.Ldind_Ref),
                new(OpCodes.Stfld, Field(typeof(FacilityManagement),nameof(FacilityManagement.Singleton.CustomText))),
                new(OpCodes.Ret),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }

    [HarmonyPatch(typeof(Exiled.API.Features.Intercom), nameof(Exiled.API.Features.Intercom.DisplayText), MethodType.Getter)]
    public static class CommandIntercomTextGetterFix
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent();

            newInstructions.AddRange(new CodeInstruction[]
            {
                new(OpCodes.Ldsfld, Field(typeof(FacilityManagement),nameof(FacilityManagement.Singleton))),
                new(OpCodes.Ldfld, Field(typeof(FacilityManagement),nameof(FacilityManagement.Singleton.CustomText))),
                new(OpCodes.Ret),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}