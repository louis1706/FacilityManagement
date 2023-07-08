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
using BaseIntercom = PlayerRoles.Voice.Intercom;
using Intercom = Exiled.API.Features.Intercom;

using Mirror;
using YamlDotNet.Core.Tokens;

namespace FacilityManagement.Patches.IntercomText
{
    public class IntercomUpdateTextPatch
    {
        private static float Timer = 0f;

        [HarmonyPatch(typeof(BaseIntercom), nameof(BaseIntercom.Update))]
        public static class RemoveBaseGameOverrideText
        {
            private static void Prefix(BaseIntercom __instance)
            {
                try
                {
                    IntercomDisplay.IcomText value = Intercom.State switch
                    {
                        IntercomState.Ready => IntercomDisplay.IcomText.Ready,
                        IntercomState.Starting => IntercomDisplay.IcomText.Wait,
                        IntercomState.InUse => Intercom.Speaker?.IsBypassModeEnabled ?? false ? IntercomDisplay.IcomText.TrasmittingBypass : IntercomDisplay.IcomText.Transmitting,
                        IntercomState.Cooldown => IntercomDisplay.IcomText.Restarting,
                        _ => IntercomDisplay.IcomText.Unknown,
                    };
                    if (FacilityManagement.Singleton.Config.IntercomRefresh is not null)
                    {
                        Timer += Time.deltaTime;

                        if (Timer <= 1f)
                            return;

                        Timer = 0f;
                    }
                    if (!string.IsNullOrEmpty(FacilityManagement.Singleton.CustomText))
                        value = IntercomDisplay.IcomText.Unknown;

                    if (ServerConsole.singleton.NameFormatter.Commands is null || FacilityManagement.Singleton.Config.CustomText is null || !FacilityManagement.Singleton.Config.CustomText.TryGetValue(value, out string content) || string.IsNullOrEmpty(content))
                        return;
                    if (!ServerConsole.singleton.NameFormatter.TryProcessExpression(content, "FacilityManagement", out string result))
                    {
                        Log.Error(result);
                    }
                    IntercomDisplay._singleton.Network_overrideText = result;
                }
                catch (Exception ex)
                {
                    Log.Error($"Yamato Issue {ex}");
                }
            }
        }
    }
}

