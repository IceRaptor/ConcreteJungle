﻿using BattleTech;
using ConcreteJungle.Extensions;
using ConcreteJungle.Helper;
using CustAmmoCategories;
using System;
using System.Collections.Generic;
using UnityEngine;
using us.frostraptor.modUtils;

namespace ConcreteJungle.Sequence
{
    public class ExplosionAmbushSequence : MultiSequence
    {
        private Stack<Vector3> AmbushPositions { get; set; }
        
        private Stack<AOEBlastDef> AmbushBlasts { get; set; }

        private Team AmbushTeam { get; set; }

        public override bool IsValidMultiSequenceChild {  get { return false;  } }

        public override bool IsParallelInterruptable { get { return false; } }
        
        public override bool IsCancelable { get { return false; } }
        

        public override bool IsComplete { get { return this.state == AmbushExplosionSequenceState.Finished; } }

        public ExplosionAmbushSequence(CombatGameState combat, List<Vector3> ambushOrigins, 
            List<AOEBlastDef> ambushBlasts, Team team) : base(combat)
        {
            this.AmbushTeam = team;
            this.AmbushPositions = new Stack<Vector3>(ambushOrigins);
            this.AmbushBlasts = new Stack<AOEBlastDef>(ambushBlasts);
            Mod.Log.Debug($"Positions count: {AmbushPositions.Count}  blasts count: {AmbushBlasts.Count}");
        }

        public override void OnAdded()
        {
            base.OnAdded();
            Mod.Log.Debug("Starting new AmbushExplosionSequence.");
            this.SetState(AmbushExplosionSequenceState.Taunting);
        }

        public override void OnUpdate()
        {
            Mod.Log.Trace($"Updating AmbushExplosionSequence in state: {this.state}");
            base.OnUpdate();
            this.timeInCurrentState += Time.deltaTime;
            switch (this.state)
            {
                case AmbushExplosionSequenceState.Taunting:
                    this.Taunt();
                    if (this.timeInCurrentState > this.timeToTaunt)
                    {
                        this.SetState(AmbushExplosionSequenceState.Attacking);
                    }
                    break;
                case AmbushExplosionSequenceState.Attacking:
                    this.ResolveNextBlast();
                    if (this.AmbushPositions.Count == 0)
                    {
                        this.SetState(AmbushExplosionSequenceState.Finished);
                    }
                    break;
                case AmbushExplosionSequenceState.Finished:
                    break;
                default:
                    return;
            }
        }

        private void SetState(AmbushExplosionSequenceState newState)
        {
            if (this.state == newState) return;

            this.state = newState;
            this.timeInCurrentState = 0f;
            switch(newState)
            {
                case AmbushExplosionSequenceState.Attacking:
                    Mod.Log.Debug("Invoking CAC for blast effects");
                    break;
                case AmbushExplosionSequenceState.Finished:
                    Mod.Log.Debug("Finished with AmbushExplosionSequence");
                    base.ClearCamera();
                    return;
                default:
                    return;

            }
        }

        private void Taunt()
        {
            if (!hasTaunted)
            {
                // Create a quip
                Guid g = Guid.NewGuid();
                QuipHelper.PlayQuip(ModState.Combat, g.ToString(),
                    this.AmbushTeam, "IED Ambush", Mod.Config.Quips.ExplosiveAmbush, this.timeToTaunt * 3f);
                hasTaunted = true;
            }
        }


        private void ResolveNextBlast()
        {
            this.timeSinceLastExplosion += Time.deltaTime;
            if (this.timeSinceLastExplosion > this.timeBetweenExplosions)
            {
                Vector3 blastOrigin = this.AmbushPositions.Pop();
                AOEBlastDef blastDef = this.AmbushBlasts.Pop();
                Mod.Log.Debug($"Spawning blast at position: {blastOrigin} that does {blastDef.Damage} damage, {blastDef.Heat} heat, and {blastDef.Stability} stability.");

                ExplosionAPIHelper.AoEExplode(
                    Mod.Config.ExplosionAmbush.VFX, Vector3.one * 50f, 20f, Mod.Config.ExplosionAmbush.SFX,
                    blastOrigin, blastDef.Radius, blastDef.Damage, blastDef.Heat, blastDef.Stability,
                    new List<EffectData>(), false, 
                    blastDef.FireRadius, blastDef.FireStrength, blastDef.FireChance, blastDef.FireDurationNoForest,
                    string.Empty, Vector3.zero, string.Empty, 0, 0);

                this.timeSinceLastExplosion = 0f;
            }

        }

        private float timeInCurrentState;

        private bool hasTaunted = false;
        private readonly float timeToTaunt = 1f;

        private float timeSinceLastExplosion = 0f;
        private readonly float timeBetweenExplosions = 0.5f;

        private AmbushExplosionSequenceState state;

        private enum AmbushExplosionSequenceState
        {
            NotSet,
            Taunting,
            Attacking,
            Finished
        }
    }
}
