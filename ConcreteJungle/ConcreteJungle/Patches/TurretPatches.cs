﻿using BattleTech;
using Harmony;

namespace ConcreteJungle.Patches
{
    // Attempt to make turrets that are trap turrets invulnerable
    [HarmonyPatch(typeof(Turret), "HandleDeath")]
    static class Turret_HandleDeath
    {
        static bool Prefix(Turret __instance)
        {
            if (__instance != null && 
                ModState.AmbushTurretGUIDtoBuilding.ContainsKey(__instance.GUID) &&
                ModState.AmbushTurretGUIDtoBuilding[__instance.GUID].GUID != ModState.KillingLinkedUnitsSource)
            {
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(Turret), "FlagForDeath")]
    static class Turret_FlagForDeath
    {
        static bool Prefix(Turret __instance)
        {
            if (__instance != null && 
                ModState.AmbushTurretGUIDtoBuilding.ContainsKey(__instance.GUID) &&
                ModState.AmbushTurretGUIDtoBuilding[__instance.GUID].GUID != ModState.KillingLinkedUnitsSource)
            {
                return false;
            }                

            return true;
        }
    }
}
