using HarmonyLib;
using Il2Cpp;
using System.Collections.Generic;
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace PaintTheTownRedMenu
{
    [HarmonyPatch]
    public static class GameObjectManager
    {
        public static PTTRPlayer? LocalPlayer { get; private set; }
        public static List<Enemy> Enemies { get; } = [];
        public static List<Weapon> Weapons { get; } = [];
        public static List<WorldButton> WorldButtons { get; } = [];
        public static List<Cannon> Cannons { get; } = [];


        [HarmonyPatch(typeof(PTTRPlayer), nameof(PTTRPlayer.Awake)), HarmonyPostfix]
        private static void PTTRPlayer_Awake(PTTRPlayer __instance)
        {
            if (__instance == null) return;
            LocalPlayer = __instance;
        }


        [HarmonyPatch(typeof(PTTRPlayer), nameof(PTTRPlayer.OnDestroy)), HarmonyPostfix]
        private static void PTTRPlayer_OnDestroy()
        {
            LocalPlayer = null;
        }

        [HarmonyPatch(typeof(Enemy), nameof(Enemy.Start)), HarmonyPostfix]
        private static void Enemy_Start(Enemy __instance)
        {
            if (__instance == null) return;
            Enemies.Add(__instance);
        }

        [HarmonyPatch(typeof(Enemy), nameof(Enemy.OnDestroy)), HarmonyPostfix]
        private static void Enemy_OnDestroy(Enemy __instance)
        {
            if (__instance == null) return;
            Enemies.Remove(__instance);
        }

        [HarmonyPatch(typeof(Weapon), nameof(Weapon.Start)), HarmonyPostfix]
        private static void Weapon_Start(Weapon __instance)
        {
            if (__instance == null) return;
            Weapons.Add(__instance);
        }

        [HarmonyPatch(typeof(Weapon), nameof(Weapon.Break)), HarmonyPostfix]
        private static void Weapon_Break(Weapon __instance)
        {
            if (__instance == null) return;
            Weapons.Remove(__instance);
        }

        [HarmonyPatch(typeof(WorldButton), nameof(WorldButton.OnEnable)), HarmonyPostfix]
        private static void WorldButton_Init(WorldButton __instance)
        {
            if (!__instance) return;
            WorldButtons.Add(__instance);
            Cannon cannon = __instance.GetComponent<Cannon>();
            if (cannon != null) Cannons.Add(cannon);
        }

        [HarmonyPatch(typeof(WorldButton), "OnDisable")]
        private static void WorldButton_OnDestroy(WorldButton __instance)
        {
            if (__instance == null) return;
            WorldButtons.Remove(__instance);
            Cannon cannon = __instance.GetComponent<Cannon>();
            if (cannon != null) Cannons.Remove(cannon);
        }
    }
}