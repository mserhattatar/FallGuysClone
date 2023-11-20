using Game.Scripts.Manager;
using UnityEngine;

namespace Game.Scripts.Pool
{
    public abstract class PooledObjectBehaviour : AbstractMonoBehaviour
    {
        private ObjectPool _objectPool;
        private Transform _parentTransform;
        public float FirstScale { get; private set; }
        public PoolObjectType PoolObjectType { get; private set; }
        public bool IsInPool { get; private set; }

        protected override void OnDisable()
        {
            base.OnDisable();
            OnReleaseToPool();
        }

        protected override void OnDestroy()
        {
            OnDestroyPooledObject();
            base.OnDestroy();
        }

        // invoked when created an item from the object pool
        public virtual void OnCreatedFromPool(ObjectPool objectPool, Transform parentTransform,
            PoolObjectType poolObjectType)
        {
            _parentTransform = parentTransform;
            _objectPool = objectPool;
            PoolObjectType = poolObjectType;
            FirstScale = transform.localScale.x;
            OnReleaseToPool();
        }

        // invoked when returning an item to the object pool
        public virtual void OnReleaseToPool()
        {
            Transform transform1;
            (transform1 = transform).SetParent(_parentTransform);
            transform1.localScale = Vector3.one * FirstScale;
            IsInPool = true;
            gameObject.SetActive(false);
        }

        // invoked when retrieving the next item from the object pool
        public void OnGetFromPool(bool activate)
        {
            IsInPool = false;
            if (activate) gameObject.SetActive(true);
        }

        // invoked when we exceed the maximum number of pooled items (i.e. destroy the pooled object)
        private void OnDestroyPooledObject()
        {
            _objectPool?.OnDestroyPooledObject(this);
        }
    }
}