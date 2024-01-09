using System;
using Game.Scripts.Base;
using ThirdPartyAssets.Joystick_Pack.Scripts.Base;
using ThirdPartyAssets.Joystick_Pack.Scripts.Joysticks;
using UnityEngine;

namespace Game.Scripts.Concretes
{
    public class Player : CharacterBase
    {
        private const float Speed = 5;
        private const float RotationSpeed = 100;
        private Joystick _joystick;
        private Action _playerFinished;

        private void FixedUpdate()
        {
            if (CanMove)
                Movement();
        }

        private void Movement()
        {
            transform.Translate(Vector3.forward * (Time.deltaTime * Speed * _joystick.Vertical));
            transform.Rotate(Vector3.up, Time.deltaTime * RotationSpeed * _joystick.Horizontal);
        }
        
        protected override float SetRunAniSpeed() =>
            _joystick.Vertical != 0 ? Math.Abs(_joystick.Vertical) : Math.Abs(_joystick.Horizontal);

        public void SetPlayer(FloatingJoystick floatingJoystick, string playerName, Transform spawnPoint,
            Transform parentTransform, Action playerFinished)
        {
            _joystick = floatingJoystick;
            CharacterName = playerName;
            transform.localPosition = spawnPoint.position;
            transform.SetParent(parentTransform);
            _playerFinished += playerFinished;
        }

        protected override void OnFinishLine()
        {
            base.OnFinishLine();
            _playerFinished?.Invoke();
        }

        public override void OnReleaseToPool()
        {
            _playerFinished = null;
            base.OnReleaseToPool();
        }
    }
}