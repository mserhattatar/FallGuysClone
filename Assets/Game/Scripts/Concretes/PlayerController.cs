using System;
using Game.Scripts.Manager;
using ThirdPartyAssets.Joystick_Pack.Scripts.Base;
using UnityEngine;

namespace Game.Scripts.Concretes
{
    public class PlayerController : Character
    {
        private const float Speed = 5;
        private const float RotationSpeed = 120;
        [SerializeField] private Joystick joystick;

        private void FixedUpdate()
        {
            if (!CanMove)
                return;

            transform.Translate(Vector3.forward * Time.deltaTime * Speed * joystick.Vertical);
            transform.Rotate(Vector3.up, Time.deltaTime * RotationSpeed * joystick.Horizontal);
        }

        protected override float SetRunAniSpeed()
        {
            return joystick.Vertical != 0 ? Math.Abs(joystick.Vertical) : Math.Abs(joystick.Horizontal);
        }

        protected override void OnFinishLine()
        {
            GameManager.FinisLineAction?.Invoke();
        }
    }
}