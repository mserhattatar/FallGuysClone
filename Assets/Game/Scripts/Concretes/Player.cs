using System;
using Game.Scripts.Base;
using Game.Scripts.Manager;
using ThirdPartyAssets.Joystick_Pack.Scripts.Base;
using ThirdPartyAssets.Joystick_Pack.Scripts.Joysticks;
using UnityEngine;

namespace Game.Scripts.Concretes
{
    public class Player : CharacterBase
    {
        private const float Speed = 5;
        private const float RotationSpeed = 120;
        private Joystick _joystick;

        private void FixedUpdate()
        {
            if (!CanMove)
                return;

            transform.Translate(Vector3.forward * (Time.deltaTime * Speed * _joystick.Vertical));
            transform.Rotate(Vector3.up, Time.deltaTime * RotationSpeed * _joystick.Horizontal);
        }

        protected override float SetRunAniSpeed()
        {
            return _joystick.Vertical != 0 ? Math.Abs(_joystick.Vertical) : Math.Abs(_joystick.Horizontal);
        }

        public void SetPlayer(FloatingJoystick floatingJoystick)
        {
            _joystick = floatingJoystick;
        }
    }
}