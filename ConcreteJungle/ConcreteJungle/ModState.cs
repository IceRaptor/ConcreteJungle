﻿
using BattleTech;
using System.Collections.Generic;
using UnityEngine;
using static ConcreteJungle.Helper.DataLoadHelper;

namespace ConcreteJungle {

    public static class ModState {

        // -- General spawn state        
        public static List<BattleTech.Building> CandidateBuildings = new List<BattleTech.Building>();
        public static List<DataloadResourcePair> LoadedResources = new List<DataloadResourcePair>();

        // -- Teams we can use for spawning OpFors
        public static Team TargetTeam = null;
        public static Team TargetAllyTeam = null;
        public static Team HostileToAllTeam = null;
        public static Team AmbushTeam = null;

        // -- Infantry Ambush state
        public static Dictionary<string, Turret> AmbushBuildingGUIDToTurrets = new Dictionary<string, Turret>();
        public static Dictionary<string, string> AmbushTurretGUIDtoBuildingGUID = new Dictionary<string, string>();
        public static Turret CurrentTurretForLOF = null;
        public static Turret CurrentTurretForLOS = null;

        // -- General ambush state
        public static int Ambushes = 0;
        public static List<Vector3> AmbushOrigins = new List<Vector3>();
        public static List<Vector3> PotentialAmbushOrigins = new List<Vector3>();
        public static float CurrentAmbushChance = Mod.Config.Ambush.BaseChance;
        public static Lance CurrentSpawningLance = null;

        // -- General state
        public static CombatGameState Combat = null;        
        public static bool IsUrbanBiome = false;
        public static int ContractDifficulty = 0;
        
        public static ExplosionAmbushDef ExplosionAmbushDefForContract = null;
        public static InfantryAmbushDef InfantryAmbushDefForContract = null;
        public static MechAmbushDef MechAmbushDefForContract = null;
        public static VehicleAmbushDef VehicleAmbushDefForContract = null;

        public static void Reset() {
            // Reinitialize state
            CandidateBuildings.Clear();
            LoadedResources.Clear();

            TargetTeam = null;
            TargetAllyTeam = null;
            HostileToAllTeam = null;

            AmbushBuildingGUIDToTurrets.Clear();
            AmbushTurretGUIDtoBuildingGUID.Clear();

            CurrentTurretForLOF = null;
            CurrentTurretForLOS = null;

            Ambushes = 0;
            AmbushOrigins.Clear();
            PotentialAmbushOrigins.Clear();
            CurrentAmbushChance = Mod.Config.Ambush.BaseChance;
            CurrentSpawningLance = null;

            Combat = null;
            IsUrbanBiome = false;
            ContractDifficulty = 0;

            ExplosionAmbushDefForContract = null;
            InfantryAmbushDefForContract = null;
            MechAmbushDefForContract = null;
            VehicleAmbushDefForContract = null;
    }

    }

}


