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
        private static float Timer = 0f;
        public static class CommandIntercomTextSetterFix
        {
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
                    new(OpCodes.Call, Method(typeof(IntercomUpdateTextPatch), nameof(SetContent))),
                    new(OpCodes.Callvirt, PropertySetter(typeof(Intercom), nameof(Intercom.Network_state))), // Set string
                    new(OpCodes.Nop),
                    new(OpCodes.Ret),
                });

                for (int z = 0; z < newInstructions.Count; z++)
                    yield return newInstructions[z];

                ListPool<CodeInstruction>.Shared.Return(newInstructions);
            }
        }

        internal static Intercom.State SetContent(Intercom intercom, Intercom.State state)
        {
            try
            {
                if (FacilityManagement.Singleton.Config.IntercomRefresh is not null)
                {
                    Timer += Time.deltaTime;

                    if (Timer <= 1f)
                        return state;

                    Timer = 0f;
                }
                if (FacilityManagement.Singleton.CustomText is not null)
                    state = Intercom.State.Custom;
                if (ServerConsole.singleton.NameFormatter.Commands is null || FacilityManagement.Singleton.Config.CustomText is null || !FacilityManagement.Singleton.Config.CustomText.TryGetValue(state, out string content))
                    return state;
                if (!ServerConsole.singleton.NameFormatter.TryProcessExpression(content, "FacilityManagement", out string result))
                {
                    Log.Error(result);
                    return state;
                }
                intercom.Network_intercomText = result;
                return Intercom.State.Custom;
            }
            catch (Exception ex)
            {
                Log.Error($"Yamato Issue {ex}");
                return state;
            }
        }
    }
}

