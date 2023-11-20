using Cinemachine;
using Game.Scripts.Container;
using Game.Scripts.Controller;
using UnityEngine;

namespace Game.Scripts.Manager
{
    public class CameraController : ComponentContainerBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera thirdPersonCamera;
        private PlayerController _playerController;

        private void Start()
        {
            SetThirdPersonCamera();
        }

        public override void ContainerOnAwake()
        {
            base.ContainerOnAwake();
            _playerController = MainContainer.GetContainerComponent(nameof(PlayerController)) as PlayerController;
        }

        private void SetThirdPersonCamera()
        {
            var newPlayer = _playerController.PlayerTransform;
            thirdPersonCamera.Follow = newPlayer;
            thirdPersonCamera.LookAt = newPlayer;
        }
    }
}