using HarmonyLib;
using System;
using Exiled.API.Features;
using UnityEngine;
using NorthwoodLib.Pools;
using System.Collections.Generic;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

using PlayerRoles.Voice;
using VoiceChat;
using Intercom = PlayerRoles.Voice.Intercom;
using Mirror;
using YamlDotNet.Core.Tokens;

namespace FacilityManagement.Patches
{
    public class IntercomUpdateTextPatch
    {
        private static float Timer = 0f;


        [HarmonyPatch(typeof(IntercomDisplay), nameof(IntercomDisplay.GetTranslation))]
        public static class CommandIntercomTextSetterFix
        {
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
            {
                Label returnLabel = generator.DefineLabel();

                List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent();

                newInstructions.AddRange(new CodeInstruction[]
                {
                    new(OpCodes.Ldarg_0), // this
                    new(OpCodes.Ldarg_1), // IntercomDisplay.IcomText
                    new(OpCodes.Call, Method(typeof(IntercomUpdateTextPatch), nameof(SetContent))),
                    new(OpCodes.Ldstr, "UwU"),
                    new(OpCodes.Ret),
                });


                for (int z = 0; z < newInstructions.Count; z++)
                {
                    Log.Info($"newInstruction {newInstructions[z].opcode} , {newInstructions[z].operand}");
                    yield return newInstructions[z];
                }
                ListPool<CodeInstruction>.Shared.Return(newInstructions);
            }
        }

        internal static void SetContent(IntercomDisplay intercom, IntercomDisplay.IcomText value)
        {
            try
            {
                if (FacilityManagement.Singleton.Config.IntercomRefresh is not null)
                {
                    Timer += Time.deltaTime;

                    if (Timer <= 1f)
                        return;

                    Timer = 0f;
                }
                if (FacilityManagement.Singleton.CustomText is not null)
                    value = IntercomDisplay.IcomText.Unknown;

                if (ServerConsole.singleton.NameFormatter.Commands is null || FacilityManagement.Singleton.Config.CustomText is null || !FacilityManagement.Singleton.Config.CustomText.TryGetValue(value, out string content))
                    return;
                if (!ServerConsole.singleton.NameFormatter.TryProcessExpression(content, "FacilityManagement", out string result))
                {
                    Log.Error(result);
                }
                intercom.Network_overrideText = result;
            }
            catch (Exception ex)
            {
                Log.Error($"Yamato Issue {ex}");
            }
        }
    }
}

