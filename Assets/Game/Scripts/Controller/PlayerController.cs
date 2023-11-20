using Game.Scripts.Concretes;
using Game.Scripts.Container;
using Game.Scripts.Manager;
using Game.Scripts.Pool;
using ThirdPartyAssets.Joystick_Pack.Scripts.Joysticks;
using UnityEngine;

namespace Game.Scripts.Controller
{
    public class PlayerController : ComponentContainerBehaviour
    {
        private LevelController _levelController;
        private Player _player;
        private PoolController _poolController;
        [SerializeField] private  FloatingJoystick _floatingJoystick;
        public Transform PlayerTransform => _player.transform;

        public override void ContainerOnAwake()
        {
            base.ContainerOnAwake();
            _poolController = MainContainer.GetContainerComponent(nameof(PoolController)) as PoolController;
            _levelController = MainContainer.GetContainerComponent(nameof(LevelController)) as LevelController;
        }

        public override void ContainerDoAfterAwake()
        {
            base.ContainerDoAfterAwake();
            CreatePlayer();
        }

        protected override void RegisterEvents()
        {
            base.RegisterEvents();
            _levelController.GameStarted += StartPlayer;
        }

        protected override void UnRegisterEvents()
        {
            base.UnRegisterEvents();
            _levelController.GameStarted -= StartPlayer;
        }

        private void CreatePlayer()
        {
            _player = _poolController.GetFromPool(PoolObjectType.PlayerCharacter, false) as Player;
            _player!.transform.localPosition = Vector3.zero;
            _player.transform.SetParent(transform);
            _player.SetPlayer(_floatingJoystick);
        }

        private void StartPlayer()
        {
            _player.gameObject.SetActive(true);
        }
    }
}