using Game.Scripts.Container;
using Game.Scripts.Manager;
using UnityEngine;

namespace Game.Scripts.Controller
{
    public class LevelController : ComponentContainerBehaviour
    {
        [SerializeField] private Transform targetTransform;
        public Transform TargetTransform => targetTransform;

        private PlayerController _playerController;
        private OpponentsController _opponentsController;
        private GameManager _gameManager;

        public override void ContainerDoAfterAwake()
        {
            base.ContainerDoAfterAwake();
            _playerController = MainContainer.GetContainerComponent(nameof(PlayerController)) as PlayerController;
            _opponentsController =
                MainContainer.GetContainerComponent(nameof(OpponentsController)) as OpponentsController;
            _gameManager = MainContainer.GetContainerComponent(nameof(GameManager)) as GameManager;
        }

        public void StartGame()
        {
            _playerController.StartGame(LevelEnded);
            _opponentsController.StartGame();
        }

        private void LevelEnded()
        {
            _gameManager.LevelEnded();
        }
    }
}