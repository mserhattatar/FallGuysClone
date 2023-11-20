using System.Collections.Generic;
using Game.Scripts.Container;
using Game.Scripts.Manager;
using UnityEngine;

namespace Game.Scripts.Pool
{
    public class PoolController : ComponentContainerBehaviour
    {
        [SerializeField] private List<PoolCollection> poolCollections;
        private readonly Dictionary<PoolObjectType, ObjectPool> _objectPoolsDictionary = new();
        private GameSceneManager _sceneManager;

        public override void ContainerOnAwake()
        {
            _sceneManager = MainContainer.GetContainerComponent(nameof(GameSceneManager)) as GameSceneManager;
            _sceneManager!.SceneChangingDelegate += ReleaseToPool;
        }

        public override void ContainerDoAfterAwake()
        {
            CreatePool();
        }

        protected override void UnRegisterEvents()
        {
            _sceneManager!.SceneChangingDelegate -= ReleaseToPool;
        }

        protected override void DoKill()
        {
        }

        private void CreatePool()
        {
            foreach (var pool in poolCollections)
                _objectPoolsDictionary.Add(pool.poolObjectType,
                    new ObjectPool(pool.prefab, pool.amount, transform, pool.poolObjectType));
        }

        public PooledObjectBehaviour GetFromPool(PoolObjectType objectType, bool activate = true)
        {
            return _objectPoolsDictionary[objectType].GetPooledObject(activate);
        }

        public void ReleaseToPool(PooledObjectBehaviour obj)
        {
            obj.OnReleaseToPool();
        }

        public void ReleaseToPool()
        {
            foreach (var objectPool in _objectPoolsDictionary.Values) objectPool.ReleaseAllPoolObj();
        }
    }
}