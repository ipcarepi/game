﻿using System.Collections.Generic;
using UnityEngine;

namespace CoverShooter
{
    /// <summary>
    /// Spawns prefab instances on various character events.
    /// </summary>
    [RequireComponent(typeof(CharacterMotor))]
    public class CharacterEffects : BaseEffects, ICharacterPhysicsListener, ICharacterHealthListener
    {
        /// <summary>
        /// Effect prefab to instantiate on each character step.
        /// </summary>
        [Tooltip("Effect prefab to instantiate on each character step.")]
        public GameObject Step;

        /// <summary>
        /// Effect prefab to instantiate when the character lands on ground.
        /// </summary>
        [Tooltip("Effect prefab to instantiate when the character lands on ground.")]
        public GameObject Land;

        /// <summary>
        /// Effect prefab to instantiate when the character dies.
        /// </summary>
        [Tooltip("Effect prefab to instantiate when the character dies.")]
        public GameObject Death;

        /// <summary>
        /// Effect prefab to instantiate when the character resurrects.
        /// </summary>
        [Tooltip("Effect prefab to instantiate when the character resurrects.")]
        public GameObject Resurrection;

        /// <summary>
        /// Effect prefab to instantiate at the beginning of a jump.
        /// </summary>
        [Tooltip("Effect prefab to instantiate at the beginning of a jump.")]
        public GameObject Jump;

        /// <summary>
        /// Effect prefab to instantiate when the character is hurt.
        /// </summary>
        [Tooltip("Effect prefab to instantiate when the character is hurt.")]
        public GameObject Hurt;

        /// <summary>
        /// Effect prefab to instantiate when the character blocks a melee attack.
        /// </summary>
        [Tooltip("Effect prefab to instantiate when the character blocks a melee attack.")]
        public GameObject Block;

        /// <summary>
        /// Effect prefab to instantiate when the character is hit.
        /// </summary>
        [Tooltip("Effect prefab to instantiate when the character is hit.")]
        public GameObject Hit;

        /// <summary>
        /// Effect prefab to instantiate when the character is dealt a lot of damage by a hit.
        /// </summary>
        [Tooltip("Effect prefab to instantiate when the character is dealt a lot of damage by a hit.")]
        public GameObject BigHit;

        /// <summary>
        /// Damage that has to be dealt to play big hit effect.
        /// </summary>
        [Tooltip("Damage that has to be dealt to play big hit effect.")]
        public float BigDamageThreshold = 50;

        private CharacterMotor _motor;
        private float _hurtSoundTimer;
        private float _fallSoundTimer;

        private void Awake()
        {
            _motor = GetComponent<CharacterMotor>();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            if (_hurtSoundTimer > -float.Epsilon)
                _hurtSoundTimer -= Time.deltaTime;

            if (_fallSoundTimer > -float.Epsilon)
                _fallSoundTimer -= Time.deltaTime;
        }

        /// <summary>
        /// Instantiates the landing effect when the character lands.
        /// </summary>
        public void OnLand()
        {
            if (_fallSoundTimer <= 0 && _motor.IsAlive)
            {
                _fallSoundTimer = 0.4f;
                Instantiate(Land, transform.position);
            }
        }

        /// <summary>
        /// Instantiates the hit effect when the character is hit.
        /// </summary>
        public void OnTakenHit(Hit hit)
        {
            if (!_motor.IsAlive)
                return;

            if (_hurtSoundTimer < float.Epsilon)
            {
                _hurtSoundTimer = 0.5f;
                Instantiate(Hurt, hit.Position);
            }

            if (BigHit != null && hit.Damage >= BigDamageThreshold)
                Instantiate(BigHit, hit.Position);
            else if (Hit != null)
                Instantiate(Hit, hit.Position);
        }

        /// <summary>
        /// Instantiates the step effect.
        /// </summary>
        public void OnFootstep(Vector3 position)
        {
        }

        /// <summary>
        /// Instantiates the death effect.
        /// </summary>
        public void OnDead()
        {
            Instantiate(Death, transform.position);
        }

        /// <summary>
        /// Instantiates the resurrection event.
        /// </summary>
        public void OnResurrect()
        {
            Instantiate(Resurrection, transform.position);
        }

        /// <summary>
        /// Instantiates the block event.
        /// </summary>
        public void OnBlock(Hit hit)
        {
            Instantiate(Block, hit.Position);
        }

        /// <summary>
        /// Instantiates the jump effect.
        /// </summary>
        public void OnJump()
        {
            if (_motor.IsAlive)
                Instantiate(Jump, transform.position);
        }
    }
}
