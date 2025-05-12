using HarmonyLib;
using System;
using Exiled.API.Features;
using System.Collections.Generic;
using UnityEngine;
using NorthwoodLib.Pools;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;
using Intercom = Exiled.API.Features.Intercom;
using BaseIntercom = PlayerRoles.Voice.Intercom;

namespace FacilityManagement.Patches.IntercomText
{
    [HarmonyPatch(typeof(BaseIntercom), nameof(BaseIntercom.Start))]
    public class NameFormaterPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            newInstructions.Insert(0, new(OpCodes.Call, Method(typeof(NameFormaterPatch), nameof(AddCustomInterpolatedCommand))));

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
        public static void AddCustomInterpolatedCommand()
        {
            try
            {
                if (ServerConsole.Singleton.NameFormatter.Commands.ContainsKey("intercom_speech_remaining_time"))
                    return;
                Utils.CommandInterpolation.InterpolatedCommandFormatter interpolatedCommandFormatter = ServerConsole.Singleton.NameFormatter;

                // Int
                interpolatedCommandFormatter.Commands.Add("intercom_speech_remaining_time", (List<string> args) => Mathf.CeilToInt(Intercom.SpeechRemainingTime).ToString());
                interpolatedCommandFormatter.Commands.Add("intercom_remaining_cooldown", (List<string> args) => Mathf.CeilToInt((float)Intercom.RemainingCooldown).ToString());
                // String
                interpolatedCommandFormatter.Commands.Add("intercom_speaker_nickname", (List<string> args) => $"{Intercom.Speaker?.DisplayNickname}");
                interpolatedCommandFormatter.Commands.Add("intercom_custom_text", (List<string> args) => FacilityManagement.Singleton.CustomText);
                // Bool
                interpolatedCommandFormatter.Commands.Add("intercom_is_in_use", (List<string> args) => (Intercom.Speaker is not null).ToString());
                
                // CustomText replace
                ServerConsole.Singleton.NameFormatter = interpolatedCommandFormatter;

            }
            catch (Exception ex)
            {
                Log.Error($"AddCustomInterpolatedCommand : {ex}\n {ex.StackTrace}");
            }
        }
    }
}

