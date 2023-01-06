namespace FacilityManagement
{
    using Exiled.Events.Handlers;
    using HarmonyLib;
    using MEC;
    using System;
    using System.Collections.Generic;

    public class FacilityManagement : Exiled.API.Features.Plugin<Config>
    {
	    public static FacilityManagement Singleton;

	    public override string Name => "FacilityManagement";
	    public override string Prefix => "FacilityManagement";
	    public override string Author => "Yamato#8987";
        public override Version Version { get; } = new(1,0,0);
        public override Version RequiredExiledVersion  { get; } = AutoUpdateExiledVersion.AutoUpdateExiledVersion.RequiredExiledVersion;

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
            
            Player.Shooting += EventHandlers.OnShooting;
            Player.UsingMicroHIDEnergy += EventHandlers.OnUsingMicroHIDEnergy;
            Player.UsingRadioBattery += EventHandlers.OnUsingRadioBattery;
            Player.Spawned += EventHandlers.OnSpawned;
            Player.Hurting += EventHandlers.OnHurting;
            
            Warhead.Detonated += EventHandlers.OnDetonated;
        }
        private void UnRegisterEvents()
        {
            Server.RoundStarted -= EventHandlers.OnRoundStarted;

            Player.Shooting -= EventHandlers.OnShooting;
            Player.UsingMicroHIDEnergy -= EventHandlers.OnUsingMicroHIDEnergy;
            Player.UsingRadioBattery -= EventHandlers.OnUsingRadioBattery;
            Player.Spawned -= EventHandlers.OnSpawned;
            Player.Hurting -= EventHandlers.OnHurting;

            Warhead.Detonated -= EventHandlers.OnDetonated;

            EventHandlers = null;
        }

        private void RegisterPatch()
        {
            try
            {
                Harmony = new(Author + "." + Name + ++patchCounter);
                Harmony.PatchAll();
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
