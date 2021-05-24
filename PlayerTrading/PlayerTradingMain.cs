﻿using HarmonyLib;
using BepInEx;
using UnityEngine;
using System.Reflection;
using System;
using BepInEx.Configuration;

namespace PlayerTrading
{

    [BepInPlugin("projjm.playerTrading", "Player Trading", "1.1.0")]
    public class PlayerTradingMain : BaseUnityPlugin
    {
        public static ConfigEntry<KeyCode> EditWindowLayoutKey;
        public static ConfigEntry<Vector2> ToGiveUserOffset;
        public static ConfigEntry<Vector2> ToReceiveUserOffset;
        public static ConfigEntry<Vector2> AcceptButtonUserOffset;
        public static ConfigEntry<Vector2> CancelButtonUserOffset;


        internal readonly Harmony harmony = new Harmony("projjm.playerTrading");
        internal Assembly assembly;
        internal static event Action OnLocalPlayerChanged;

        private TradeHandler tradeHandler;

        public void Awake()
        {
            assembly = Assembly.GetExecutingAssembly();
            harmony.PatchAll(assembly);
            BindConfigs();
        }

        private void BindConfigs()
        {
            ToGiveUserOffset = Config.Bind("Offsets", "toGiveUserOffset", Vector2.zero, "Offset values for To Give trade window (Set to nothing to reset position)");
            ToReceiveUserOffset = Config.Bind("Offsets", "toReceiveUserOffset", Vector2.zero, "Offset values for To Receive trade window (Set to nothing to reset position)");
            AcceptButtonUserOffset = Config.Bind("Offsets", "acceptButtonUserOffset", Vector2.zero, "Offset values for the Accept Trade button (Set to nothing to reset position)");
            CancelButtonUserOffset = Config.Bind("Offsets", "cancelButtonUserOffset", Vector2.zero, "Offset values for the Cancel Trade button (Set to nothing to reset position)");
            EditWindowLayoutKey = Config.Bind("Keybinds", "editWindowLayoutKey", KeyCode.F11, "Key to press to enable Window Position Mode");
        }

        public void OnDestroy()
        {
            if (tradeHandler)
                Destroy(tradeHandler);

            harmony.UnpatchSelf();
        }

        private void Update()
        {
            TryAddHandler();
        }

        private void TryAddHandler()
        {
            if (tradeHandler != null || !ZNet.instance)
                return;

            GameObject PlayerTrading = new GameObject();
            tradeHandler = PlayerTrading.AddComponent<TradeHandler>();
        }

        public static void NewLocalPlayer()
        {
            OnLocalPlayerChanged?.Invoke();
        }


    }
}

