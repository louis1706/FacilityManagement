using HarmonyLib;
using System;
using Exiled.API.Features;
using System.Collections.Generic;
using UnityEngine;

namespace FacilityManagement.Patches
{
    [HarmonyPatch(typeof(ServerConsole), nameof(ServerConsole.Start))]

    public class NameFormaterPatche
    {
        public static void Postfix()
        {
            try
            {
                Utils.CommandInterpolation.InterpolatedCommandFormatter interpolatedCommandFormatter = ServerConsole.singleton.NameFormatter;
                //Int
                interpolatedCommandFormatter.Commands.Add("intercom_speech_remaining_time", (List<string> args) => Mathf.CeilToInt(Exiled.API.Features.Intercom.SpeechRemainingTime).ToString());
                interpolatedCommandFormatter.Commands.Add("intercom_remaining_cooldown", (List<string> args) => Mathf.CeilToInt(Intercom.host.remainingCooldown).ToString());
                //String
                interpolatedCommandFormatter.Commands.Add("intercom_speaker_nickname", (List<string> args) => Exiled.API.Features.Intercom.Speaker.Nickname.ToString());
                //Bool
                interpolatedCommandFormatter.Commands.Add("intercom_is_in_use", (List<string> args) => (Intercom.host.speaker is not null).ToString());
                interpolatedCommandFormatter.Commands.Add("intercom_is_admin_speaking", (List<string> args) => Intercom.AdminSpeaking.ToString());
                interpolatedCommandFormatter.Commands.Add("intercom_bypass_speaking", (List<string> args) => Intercom.host.bypassSpeaking.ToString());
                interpolatedCommandFormatter.Commands.Add("intercom_mute_player_speak", (List<string> args) => Intercom.host.Muted.ToString());

                ServerConsole.singleton.NameFormatter = interpolatedCommandFormatter;
            }
            catch (Exception ex)
            {
                Log.Error($"ServerConsole::Start Postfix : {ex}\n {ex.StackTrace}");
            }
        }
    }
}

