using System;
using Game.Scripts.Pool;

namespace Game.Scripts.Manager
{
    [Serializable]
    public class PoolCollection
    {
        public PoolObjectType poolObjectType;
        public PooledObjectBehaviour prefab;
        public int amount;

        public PoolCollection(PoolObjectType poolObjectType, PooledObjectBehaviour prefab, int amount = 10)
        {
            this.poolObjectType = poolObjectType;
            this.prefab = prefab;
            this.amount = amount;
        }
    }
}