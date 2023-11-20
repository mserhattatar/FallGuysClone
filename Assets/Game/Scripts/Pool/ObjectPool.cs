using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Pool;
using UnityEngine;

namespace Game.Scripts.Manager
{
    public class ObjectPool
    {
        private readonly PooledObjectBehaviour _objectToPool;
        private readonly Transform _parentTransform;
        private readonly List<PooledObjectBehaviour> _pooledObjects;
        private readonly PoolObjectType _poolObjectType;

        public ObjectPool(PooledObjectBehaviour objPrefab, int poolAmount, Transform parentTransform,
            PoolObjectType poolObjectType)
        {
            _parentTransform = parentTransform;
            _objectToPool = objPrefab;
            _pooledObjects = new List<PooledObjectBehaviour>();
            _poolObjectType = poolObjectType;

            if (_pooledObjects.Count > 0)
            {
                poolAmount = -_pooledObjects.Count;
                foreach (var pooledObject in _pooledObjects) pooledObject.OnReleaseToPool();
            }

            for (var i = 0; i < poolAmount; i++)
                CreateObject();
        }

        private PooledObjectBehaviour CreateObject()
        {
            var obj = Object.Instantiate(_objectToPool);
            obj.OnCreatedFromPool(this, _parentTransform, _poolObjectType);
            _pooledObjects.Add(obj);
            return obj;
        }

        public PooledObjectBehaviour GetPooledObject(bool activate)
        {
            var pooledObj = _pooledObjects.FirstOrDefault(obj => !obj.IsInPool) ?? CreateObject();

            pooledObj.OnGetFromPool(activate);

            return pooledObj;
        }

        public void ReleaseAllPoolObj()
        {
            foreach (var behaviour in _pooledObjects) behaviour.OnReleaseToPool();
        }

        public void OnDestroyPooledObject(PooledObjectBehaviour pooledObjectBehaviour)
        {
            if (_pooledObjects.Contains(pooledObjectBehaviour))
                _pooledObjects.Remove(pooledObjectBehaviour);
        }
    }
}