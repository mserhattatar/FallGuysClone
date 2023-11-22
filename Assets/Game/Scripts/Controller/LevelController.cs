using System;
using Game.Scripts.Container;
using UnityEngine;

namespace Game.Scripts.Controller
{
    public class LevelController : ComponentContainerBehaviour
    {
        [SerializeField] private Transform targetTransform;
        public Action GameStarted;
        public Transform TargetTransform => targetTransform;

        public void StartGame()
        {
            GameStarted?.Invoke();
        }
    }
}