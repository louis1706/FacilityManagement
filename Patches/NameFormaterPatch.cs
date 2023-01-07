using HarmonyLib;
using System;
using Exiled.API.Features;
using System.Collections.Generic;
using UnityEngine;
using NorthwoodLib.Pools;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;
using PlayerRoles.Voice;
using Intercom = Exiled.API.Features.Intercom;

namespace FacilityManagement.Patches
{
    [HarmonyPatch(typeof(IntercomDisplay), nameof(IntercomDisplay.Awake))]
    public class NameFormaterPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            newInstructions.InsertRange(0, new CodeInstruction[]
            {
                new(OpCodes.Nop),
                new(OpCodes.Call, Method(typeof(NameFormaterPatch),nameof(AddCustomInterpolatedCommand))),
                new(OpCodes.Nop),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
        public static void AddCustomInterpolatedCommand()
        {
            try
            {
                if (ServerConsole.singleton.NameFormatter.Commands.ContainsKey("intercom_speech_remaining_time"))
                    return;
                Utils.CommandInterpolation.InterpolatedCommandFormatter interpolatedCommandFormatter = ServerConsole.singleton.NameFormatter;

                // Int
                interpolatedCommandFormatter.Commands.Add("intercom_speech_remaining_time", (List<string> args) => Mathf.CeilToInt(Intercom.SpeechRemainingTime).ToString());
                interpolatedCommandFormatter.Commands.Add("intercom_remaining_cooldown", (List<string> args) => Mathf.CeilToInt((float)Intercom.RemainingCooldown).ToString());
                // String
                interpolatedCommandFormatter.Commands.Add("intercom_speaker_nickname", (List<string> args) => $"{Intercom.Speaker?.Nickname}");
                interpolatedCommandFormatter.Commands.Add("intercom_custom_text", (List<string> args) => FacilityManagement.Singleton.CustomText);
                // Bool
                interpolatedCommandFormatter.Commands.Add("intercom_is_in_use", (List<string> args) => (Intercom.Speaker is not null).ToString());
                
                // remove from the game
                //interpolatedCommandFormatter.Commands.Add("intercom_is_admin_speaking", (List<string> args) => Intercom.Spe.ToString());
                //interpolatedCommandFormatter.Commands.Add("intercom_bypass_speaking", (List<string> args) => Intercom.By.ToString());
                //interpolatedCommandFormatter.Commands.Add("intercom_mute_player_speak", (List<string> args) => Intercom.host.Muted.ToString());

                // CustomText replace
                ServerConsole.singleton.NameFormatter = interpolatedCommandFormatter;

            }
            catch (Exception ex)
            {
                Log.Error($"AddCustomInterpolatedCommand : {ex}\n {ex.StackTrace}");
            }
        }
    }
}

