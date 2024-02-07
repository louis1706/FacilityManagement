namespace FacilityManagement
{
    using Exiled.Events.Handlers;
    using HarmonyLib;
    using InventorySystem;
    using InventorySystem.Items;
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
        public override Version RequiredExiledVersion  { get; } = new(8,8,0);

        public EventHandlers EventHandlers { get; private set; }

        private Harmony Harmony = new("dev.Yamato");
		private int patchCounter;

        public List<CoroutineHandle> RoundCoroutines = new();
        public string CustomText = string.Empty;

        public override void OnEnabled()
        {
            Singleton = this;

            RegisterEvents();
            RegisterPatch();

            base.OnEnabled();
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
            Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            if (Config.InfiniteAmmo is not null)
                Item.ChangingAmmo += EventHandlers.OnChangingAmmo;
            if (Config.EnergyMicroHid is not 1)
                Player.UsingMicroHIDEnergy += EventHandlers.OnUsingMicroHIDEnergy;
            if (Config.EnergyRadio is not 1)
                Player.UsingRadioBattery += EventHandlers.OnUsingRadioBattery;
            Player.Spawned += EventHandlers.OnSpawned;
            if (Config.RoleTypeHumeShield is not null)
                Player.Hurting += EventHandlers.OnHurting;
            if (Config.WarheadCleanup)
                Warhead.Detonated += EventHandlers.OnDetonated;
            if (Config.CustomItem is not null)
                Player.ItemAdded += EventHandlers.CustomItem;
        }
        private void UnRegisterEvents()
        {
            Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;

            Item.ChangingAmmo -= EventHandlers.OnChangingAmmo;
            Player.UsingMicroHIDEnergy -= EventHandlers.OnUsingMicroHIDEnergy;
            Player.UsingRadioBattery -= EventHandlers.OnUsingRadioBattery;
            Player.Spawned -= EventHandlers.OnSpawned;
            Player.Hurting -= EventHandlers.OnHurting;

            Warhead.Detonated -= EventHandlers.OnDetonated;
            Player.ItemAdded -= EventHandlers.CustomItem;

            EventHandlers = null;
        }

        private void RegisterPatch()
        {
            try
            {
                Harmony = new(Author + "." + Name + ++patchCounter);
                Harmony.PatchAll();
            }
            catch (HarmonyException ex)
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
