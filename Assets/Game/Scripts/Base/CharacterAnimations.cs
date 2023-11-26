using System;
using UnityEngine;

namespace Game.Scripts.Base
{
    public class CharacterAnimations
    {
        private static readonly int RunSpeed = Animator.StringToHash("RunSpeed");
        private static readonly int Falling = Animator.StringToHash("Falling");
        private static readonly int FallingDown = Animator.StringToHash("FallingDown");
        private static readonly int StandingUp = Animator.StringToHash("StandingUp");
        private readonly Animator _cAnimator;
        private float _lastRunSpeed;

        protected internal CharacterAnimations(Animator pAnimator)
        {
            _cAnimator = pAnimator;
        }

        protected internal void SetRun(float runSpeed)
        {
            //dont use system tolerance or int
            if (Math.Abs(runSpeed - _lastRunSpeed) < 0.1f)
                return;
            _lastRunSpeed = runSpeed;
            _cAnimator.SetFloat(RunSpeed, runSpeed);
        }

        /// <summary>
        ///     Fall in to the space from ground
        /// </summary>
        protected internal void SetFalling(bool falling)
        {
            _cAnimator.SetBool(Falling, falling);
            SetStandingUp(!falling);
        }

        /// <summary>
        ///     fall in to the ground after hitting obstacles
        /// </summary>
        protected internal void SetFallingDown(bool fallingDown)
        {
            _cAnimator.SetBool(FallingDown, fallingDown);
        }

        /// <summary>
        ///     stand up after falling to space or falling down
        /// </summary>
        protected internal void SetStandingUp(bool standing)
        {
            _cAnimator.SetBool(StandingUp, standing);
        }
    }
}