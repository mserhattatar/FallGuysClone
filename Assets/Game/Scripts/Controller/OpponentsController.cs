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
        private readonly List<AIOpponent> _aiOpponents = new();
        private LevelController _levelController;

        private PoolController _poolController;
        public IEnumerable<Transform> GetAllCharacterTransform => _aiOpponents.Select(x => x.transform);

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

        private void CreateOpponents()
        {
            var opNames = ConstantsVariables.Names.ToList();
            opNames.Shuffle();

            for (var i = 0; i < ConstantsVariables.OpponentCount; i++)
            {
                var fromPool = _poolController.GetFromPool(PoolObjectType.EnemyCharacter, false) as AIOpponent;
                fromPool!.CreateAgent(opNames[i], startPoses[i].position, 50);
                fromPool.transform.SetParent(transform);
                _aiOpponents.Add(fromPool);
            }
        }

        private void StartPlayer()
        {
            _aiOpponents.ForEach(x => x.gameObject.SetActive(true));
        }
    }
}