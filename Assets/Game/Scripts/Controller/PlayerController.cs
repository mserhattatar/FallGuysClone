using System;
using Game.Scripts.Concretes;
using Game.Scripts.Container;
using Game.Scripts.Helpers;
using Game.Scripts.Manager;
using Game.Scripts.Pool;
using ThirdPartyAssets.Joystick_Pack.Scripts.Joysticks;
using UnityEngine;

namespace Game.Scripts.Controller
{
    public class PlayerController : ComponentContainerBehaviour
    {
        [SerializeField] private FloatingJoystick floatingJoystick;
        [SerializeField] private Transform spawnPoint;
        private PoolController _poolController;
        private Action _levelEnded;
        public Player GetPlayer { get; private set; }

        public override void ContainerOnAwake()
        {
            base.ContainerOnAwake();
            _poolController = MainContainer.GetContainerComponent(nameof(PoolController)) as PoolController;
        }

        public override void ContainerDoAfterAwake()
        {
            base.ContainerDoAfterAwake();
            CreatePlayer();
        }

        public void StartGame(Action levelEnded)
        {
            GetPlayer.Activate();
            _levelEnded += levelEnded;
        }

        private void CreatePlayer()
        {
            GetPlayer = _poolController.GetFromPool(PoolObjectType.PlayerCharacter, false) as Player;
            GetPlayer!.SetPlayer(floatingJoystick, ConstantsVariables.PlayerName, spawnPoint, transform,
                () => _levelEnded?.Invoke());
        }
    }
}