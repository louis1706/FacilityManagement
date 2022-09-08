using HarmonyLib;
using System;
using Exiled.API.Features;
using UnityEngine;
using NorthwoodLib.Pools;
using System.Collections.Generic;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;
using Mono.Cecil.Cil;

namespace FacilityManagement.Patches
{
    public class IntercomUpdateTextPatch
    {
        [HarmonyPatch(typeof(Intercom), nameof(Intercom.IntercomState), MethodType.Setter)]
        public static class CommandIntercomTextSetterFix
        {
            public static void Postfix(Intercom __instance, ref Intercom.State value)
            {
                __instance.Network_state = SetContent(__instance, value);
                return;
            }
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
            {
                Label returnLabel = generator.DefineLabel();

                List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent();

                newInstructions.AddRange(new CodeInstruction[]
                {
                    new(OpCodes.Nop),
                    new(OpCodes.Ldarg_0),
                    new(OpCodes.Ldarg_0),
                    new(OpCodes.Ldarg_1),
                    new(OpCodes.Ldind_U1),
                    new(OpCodes.Call, Method(typeof(IntercomUpdateTextPatch), nameof(SetContent))),
                    new(OpCodes.Callvirt, PropertySetter(typeof(Intercom), nameof(Intercom.Network_state))), // Set string
                    new CodeInstruction(OpCodes.Ret),
                });

                for (int z = 0; z < newInstructions.Count; z++)
                    yield return newInstructions[z];

                ListPool<CodeInstruction>.Shared.Return(newInstructions);
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

