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
        [SerializeField] private FloatingJoystick _floatingJoystick;
        private LevelController _levelController;
        private PoolController _poolController;
        public Player GetPlayer { get; private set; }

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
            GetPlayer = _poolController.GetFromPool(PoolObjectType.PlayerCharacter, false) as Player;
            GetPlayer!.transform.localPosition = Vector3.zero;
            GetPlayer.transform.SetParent(transform);
            GetPlayer.SetPlayer(_floatingJoystick);
        }

        private void StartPlayer()
        {
            GetPlayer.gameObject.SetActive(true);
        }
    }
}