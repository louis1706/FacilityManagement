namespace FacilityManagement
{
	using Exiled.API.Enums;
    using Exiled.Events.Handlers;
    using HarmonyLib;
    using MEC;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class FacilityManagement : Exiled.API.Features.Plugin<Config>
    {
		public static FacilityManagement Singleton;

		public override string Name => "FacilityManagement";
		public override string Prefix => "FacilityManagement";
		public override string Author => "Yamato#8987";
        public override Version Version => new(1,0,0);
        public override Version RequiredExiledVersion => new(5,2,2);

        public EventHandlers EventHandlers { get; private set; }

        private Harmony Harmony = new("dev.Yamato");
		private int patchCounter;

        public List<CoroutineHandle> RoundCoroutines = new();
        public string CustomText = string.Empty;

        public override void OnEnabled()
        {
            if (!Config.IsEnabled) return;
            Singleton = this;
            base.OnEnabled();

            RegistEvents();

            RegistPatch();

            Exiled.API.Features.Log.Info($"[OnEnabled] FacilityManagement({Version}) Enabled Complete.");
        }
        public override void OnReloaded()
        {
            if (!Config.IsEnabled) return;
            RegistEvents();
            RegistPatch();

            base.OnReloaded();
        }
        public override void OnDisabled()
        {
            base.OnDisabled();

            foreach (var cor in RoundCoroutines)
                Timing.KillCoroutines(cor);
            RoundCoroutines.Clear();

            UnRegistEvents();

            UnRegistPatch();

            Exiled.API.Features.Log.Info($"[OnDisable] SanyaRemastered({Version}) Disabled Complete.");
        }
        private void RegistEvents()
        {
            EventHandlers = new(this);
            Server.RoundStarted += EventHandlers.HandleRoundStart;
            
            Player.Shooting += EventHandlers.HandleWeaponShoot;
            Player.UsingMicroHIDEnergy += EventHandlers.HandleEnergyMicroHid;
            Player.UsingRadioBattery += EventHandlers.HandleEnergyRadio;
            Player.Spawning += EventHandlers.OnSpawning;
            Player.Hurting += EventHandlers.OnHurting;
            Player.EnteringFemurBreaker += EventHandlers.HandleFemurEnter;
            
            Scp106.Containing += EventHandlers.HandleContain106;

            Warhead.Detonated += EventHandlers.HandleWarheadDetonation;
        }
        private void UnRegistEvents()
        {
            Server.RoundStarted -= EventHandlers.HandleRoundStart;

            Player.Shooting -= EventHandlers.HandleWeaponShoot;
            Player.UsingMicroHIDEnergy -= EventHandlers.HandleEnergyMicroHid;
            Player.UsingRadioBattery -= EventHandlers.HandleEnergyRadio;
            Player.Spawning -= EventHandlers.OnSpawning;
            Player.Hurting -= EventHandlers.OnHurting;
            Player.EnteringFemurBreaker -= EventHandlers.HandleFemurEnter;

            Scp106.Containing -= EventHandlers.HandleContain106;

            Warhead.Detonated -= EventHandlers.HandleWarheadDetonation;

            EventHandlers = null;
        }

        private void RegistPatch()
        {
            try
            {
                Harmony = new(Author + "." + Name + ++patchCounter);
                Harmony.PatchAll();
            }
            catch (Exception ex)
            {
                Exiled.API.Features.Log.Error($"[RegistPatch] Patching Failed : {ex}");
            }
        }

        private void UnRegistPatch()
        {
            Harmony.UnpatchAll(Harmony.Id);
        }
    }
}