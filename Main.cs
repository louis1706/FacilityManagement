namespace FacilityManagement
{
    using CommandSystem.Commands.RemoteAdmin.MutingAndIntercom;
    using Exiled.Events.Handlers;
    using global::FacilityManagement.Patches;
    using HarmonyLib;
    using MEC;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;

    public class FacilityManagement : Exiled.API.Features.Plugin<Config>
    {
	    public static FacilityManagement Singleton;

	    public override string Name => "FacilityManagement";
	    public override string Prefix => "FacilityManagement";
	    public override string Author => "Yamato#8987";
        public override Version Version { get; } = new(1,0,0);
        public override Version RequiredExiledVersion  { get; } = new(5,2,2);

        public EventHandlers EventHandlers { get; private set; }

        private Harmony Harmony = new("dev.Yamato");
		private int patchCounter;

        public List<CoroutineHandle> RoundCoroutines = new();
        public string CustomText = string.Empty;

        public override void OnEnabled()
        {
            Singleton = this;
            base.OnEnabled();

            RegisterEvents();

            RegisterPatch();
        }
        public override void OnReloaded()
        {
            RegisterEvents();
            RegisterPatch();

            base.OnReloaded();
        }
        public override void OnDisabled()
        {
            base.OnDisabled();

            foreach (var cor in RoundCoroutines)
                Timing.KillCoroutines(cor);
            RoundCoroutines.Clear();

            UnRegisterEvents();

            UnRegisterPatch();
        }
        private void RegisterEvents()
        {
            EventHandlers = new(this);
            Server.RoundStarted += EventHandlers.OnRoundStarted;

            if (Config.InfiniteAmmo is not null)
                Player.Shooting += EventHandlers.OnShooting;
            if (Config.EnergyMicroHid.HasValue)
                Player.UsingMicroHIDEnergy += EventHandlers.OnUsingMicroHIDEnergy;
            if (Config.EnergyRadio.HasValue)
                Player.UsingRadioBattery += EventHandlers.OnUsingRadioBattery;
            if (Config.RoleTypeHumeShield is not null)
                Player.Spawning += EventHandlers.OnSpawning;
            if (Config.RoleTypeHumeShield is not null)
                Player.Hurting += EventHandlers.OnHurting;
            if (Config.WarheadCleanup)
                Warhead.Detonated += EventHandlers.OnDetonated;

            Player.EnteringFemurBreaker += EventHandlers.OnEnteringFemurBreaker;
            Scp106.Containing += EventHandlers.OnContaining;
        }
        private void UnRegisterEvents()
        {
            Server.RoundStarted -= EventHandlers.OnRoundStarted;
            if (Config.InfiniteAmmo is not null)
                Player.Shooting -= EventHandlers.OnShooting;
            if (Config.EnergyMicroHid.HasValue)
                Player.UsingMicroHIDEnergy -= EventHandlers.OnUsingMicroHIDEnergy;
            if (Config.EnergyRadio.HasValue)
                Player.UsingRadioBattery -= EventHandlers.OnUsingRadioBattery;
            if (Config.RoleTypeHumeShield is not null)
                Player.Spawning -= EventHandlers.OnSpawning;
            if (Config.RoleTypeHumeShield is not null)
                Player.Hurting -= EventHandlers.OnHurting;
            if (Config.WarheadCleanup)
                Warhead.Detonated -= EventHandlers.OnDetonated;


            Player.EnteringFemurBreaker -= EventHandlers.OnEnteringFemurBreaker;
            Scp106.Containing -= EventHandlers.OnContaining;

            EventHandlers = null;
        }

        private void RegisterPatch()
        {
            try
            {
                Harmony = new(Author + "." + Name + ++patchCounter);
                if (Config.CustomText is not null)
                {
                    Harmony.Patch(typeof(IntercomTextCommand).GetMethod(nameof(IntercomTextCommand.Execute)), transpiler: new HarmonyMethod(typeof(IntercomTextCommandFix).GetMethod(nameof(IntercomTextCommandFix.Transpiler))));
                    
                    Harmony.Patch(typeof(Exiled.API.Features.Intercom).GetProperty(nameof(Exiled.API.Features.Intercom.DisplayText)).SetMethod, transpiler: new HarmonyMethod(typeof(CommandIntercomTextSetterFix).GetMethod(nameof(CommandIntercomTextSetterFix.Transpiler))));
                    Harmony.Patch(typeof(Exiled.API.Features.Intercom).GetProperty(nameof(Exiled.API.Features.Intercom.DisplayText)).GetMethod, transpiler: new HarmonyMethod(typeof(CommandIntercomTextGetterFix).GetMethod(nameof(CommandIntercomTextGetterFix.Transpiler))));

                    Harmony.Patch(typeof(Intercom).GetProperty(nameof(Intercom.IntercomState)).GetMethod, transpiler: new HarmonyMethod(typeof(CommandIntercomTextSetterFix).GetMethod(nameof(CommandIntercomTextSetterFix.Transpiler))));

                    Harmony.Patch(typeof(Intercom).GetMethod(nameof(Intercom.Start)), transpiler: new HarmonyMethod(typeof(NameFormaterPatch).GetMethod(nameof(NameFormaterPatch.Transpiler))));
                }
            }
            catch (Exception ex)
            {
                Exiled.API.Features.Log.Error($"[RegisterPatch] Patching Failed : {ex}");
            }
        }

        private void UnRegisterPatch()
        {
            Harmony.UnpatchAll(Harmony.Id);
        }
    }
}
