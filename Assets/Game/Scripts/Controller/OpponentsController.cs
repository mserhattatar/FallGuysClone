using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Concretes;
using Game.Scripts.Container;
using Game.Scripts.Helpers;
using Game.Scripts.Manager;
using Game.Scripts.Pool;
using UnityEngine;

namespace Game.Scripts.Controller
{
    public class OpponentsController : ComponentContainerBehaviour
    {
        [SerializeField] private Transform[] startPoses;
        private LevelController _levelController;

        private PoolController _poolController;
        public List<AIOpponent> GetAllOpponents { get; } = new();

        public override void ContainerOnAwake()
        {
            base.ContainerOnAwake();
            _poolController = MainContainer.GetContainerComponent(nameof(PoolController)) as PoolController;
            _levelController = MainContainer.GetContainerComponent(nameof(LevelController)) as LevelController;
        }

        public override void ContainerDoAfterAwake()
        {
            base.ContainerDoAfterAwake();
            CreateOpponents();
        }

        public void StartGame()
        {
            StartOpponents();
        }

        private void CreateOpponents()
        {
            var opNames = ConstantsVariables.Names.ToList();
            opNames.Shuffle();
            var targetTransform = _levelController.TargetTransform;
            for (var i = 0; i < ConstantsVariables.OpponentCount; i++)
            {
                var fromPool = _poolController.GetFromPool(PoolObjectType.EnemyCharacter, false) as AIOpponent;
                fromPool!.CreateAgent(opNames[i], startPoses[i].position, targetTransform, transform);
                GetAllOpponents.Add(fromPool);
            }
        }

        private void StartOpponents()
        {
            GetAllOpponents.ForEach(x => x.Activate());
        }
    }
}