﻿using HarmonyLib;
using System;
using System.Linq;

namespace MoreAccessoriesKOI.Patches
{
#if KK || KKS
    [HarmonyPatch(typeof(ChaFile), nameof(ChaFile.GetCoordinateBytes), new Type[0])]
    internal class CharaSavePatch
    {
        [HarmonyPriority(Priority.First)]
        internal static void Prefix(ChaFile __instance, out ChaFileAccessory.PartsInfo[][] __state)
        {
            try
            {
                var coord = __instance.coordinate;
                __state = new ChaFileAccessory.PartsInfo[coord.Length][];
                for (int outfitnum = 0, n = coord.Length; outfitnum < n; outfitnum++)
                {
                    __state[outfitnum] = coord[outfitnum].accessory.parts;
                    var lastvalidslot = Array.FindLastIndex(coord[outfitnum].accessory.parts, x => x.type != 120);
                    if (lastvalidslot < 20) continue;
                    coord[outfitnum].accessory.parts = coord[outfitnum].accessory.parts.Take(lastvalidslot + 1).ToArray();
                }
            }
            catch (Exception ex)
            {
                __state = null;
                MoreAccessories.Print($"Error occured while saving chafile coordinates {ex}", BepInEx.Logging.LogLevel.Fatal);
            }
        }

        [HarmonyPriority(Priority.First)]
        internal static void Postfix(ChaFile __instance, ChaFileAccessory.PartsInfo[][] __state)
        {
            if (__state == null) return;

            for (var i = 0; i < __state.Length; i++)
            {
                __instance.coordinate[i].accessory.parts = __state[i];
            }
        }
    }
#else
    [HarmonyPatch(typeof(ChaFile), nameof(ChaFile.GetCoordinateBytes), new Type[0])]
    internal class SavePatch
    {
        [HarmonyPriority(Priority.First)]
        internal static void Prefix(ChaFile __instance, out int __state)
        {
            var coord = __instance.coordinate;
            __state = coord.accessory.parts.Length;
            var accessories = coord.accessory.parts.ToList();
            for (var slot = accessories.Count - 1; accessories.Count > 20; slot--)
            {
                if (accessories[slot].type != 120) break;
                accessories.RemoveAt(slot);
            }
            coord.accessory.parts = accessories.ToArray();

        }

        [HarmonyPriority(Priority.First)]
        internal static void Postfix(ChaFile __instance, int __state)
        {
            var delta = __state - __instance.coordinate.accessory.parts.Length;
            if (delta > 0)
            {
                var newarray = new ChaFileAccessory.PartsInfo[delta];
                for (var i = 0; i < delta; i++)
                {
                    newarray[i] = new ChaFileAccessory.PartsInfo();
                }
                __instance.coordinate.accessory.parts = __instance.coordinate.accessory.parts.Concat(newarray).ToArray();
            }
        }
    }

#endif
    [HarmonyPatch(typeof(ChaFileCoordinate), nameof(ChaFileCoordinate.SaveFile))]
    internal class CoordSavePatch
    {
        [HarmonyPriority(Priority.First)]
        internal static void Prefix(ChaFileCoordinate __instance, out ChaFileAccessory.PartsInfo[] __state)
        {
            try
            {
                __state = __instance.accessory.parts.ToArray();

                var lastvalidslot = Array.FindLastIndex(__instance.accessory.parts, x => x.type != 120);
                if (lastvalidslot < 20) return;
                __instance.accessory.parts = __instance.accessory.parts.Take(lastvalidslot + 1).ToArray();
            }
            catch (Exception ex)
            {
                MoreAccessories.Print($"Error occured while saving coordinate {ex}", BepInEx.Logging.LogLevel.Fatal);
                __state = null;
            }
        }

        [HarmonyPriority(Priority.First)]
        internal static void Postfix(ChaFileCoordinate __instance, ChaFileAccessory.PartsInfo[] __state)
        {
            if (__state != null)
                __instance.accessory.parts = __state;
        }
    }

    [HarmonyPatch(typeof(ChaFile), nameof(ChaFile.GetStatusBytes), typeof(ChaFileStatus))]
    internal static class ChaFileStatusPatch
    {
        [HarmonyPriority(Priority.First)]
        private static void Prefix(ChaFileStatus _status, out bool[] __state)
        {
            try
            {
                __state = _status.showAccessory;
                _status.showAccessory = __state.Take(20).ToArray();
            }
            catch (Exception ex)
            {
                MoreAccessories.Print($"Error occured while saving chafile coordinates {ex}", BepInEx.Logging.LogLevel.Fatal);
                __state = null;
            }
        }
        [HarmonyPriority(Priority.First)]

        private static void Postfix(ChaFileStatus _status, bool[] __state)
        {
            if (__state != null)
                _status.showAccessory = __state;
        }
    }
}
