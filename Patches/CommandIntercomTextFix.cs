using CommandSystem;
using CommandSystem.Commands.RemoteAdmin.MutingAndIntercom;
using Exiled.API.Features;
using HarmonyLib;
using NorthwoodLib.Pools;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

using static HarmonyLib.AccessTools;

namespace FacilityManagement.Patches
{
    [HarmonyPatch(typeof(IntercomTextCommand), nameof(IntercomTextCommand.Execute))]
    public static class IntercomTextCommandFix
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            // Fix useless check from NW skillissue
            const int offset = 2;
            int index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Ret) + offset;
            
            newInstructions.RemoveRange(index, 16);

            newInstructions.Insert(index, new(OpCodes.Pop));

            // Facility Management Fix

            index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Ldloc_0);

            // remove component.CustomContent = null;
            newInstructions.RemoveRange(index, 4);

            // Add FacilityManagement.Singleton.CustomText = null; 
            newInstructions.InsertRange(index, new CodeInstruction[]
            {
                new(OpCodes.Ldsfld, Field(typeof(FacilityManagement),nameof(FacilityManagement.Singleton))),
                new(OpCodes.Ldnull),
                new(OpCodes.Stfld, Field(typeof(FacilityManagement),nameof(FacilityManagement.Singleton.CustomText))),
            });



            index = newInstructions.FindLastIndex(instruction => instruction.opcode == OpCodes.Ldloc_0);

            // remove component.CustomContent = text;
            newInstructions.RemoveRange(index, 4);

            // Add FacilityManagement.Singleton.CustomText = text;
            newInstructions.InsertRange(index, new CodeInstruction[]
            {
                new(OpCodes.Ldsfld, Field(typeof(FacilityManagement),nameof(FacilityManagement.Singleton))),
                new(OpCodes.Ldloc_0),
                new(OpCodes.Stfld, Field(typeof(FacilityManagement),nameof(FacilityManagement.Singleton.CustomText))),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}