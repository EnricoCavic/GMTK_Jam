using DPA.Generic;
using DPA.Managers.Pooling;
using System.Collections.Generic;
using UnityEngine;

namespace DPA.Managers
{
    public class ObjectPoolManager : Singleton<ObjectPoolManager>
    {
        public Dictionary<GameObject, PoolInstance> pools { get; private set; }

        private void Awake()
        {
            if (!InstanceSetup(this)) return;
            pools = new();
        }

        public PoolInstance GetPoolInstance(GameObject _prefabToPool, int _defaultCapacity = 30, int _maxSize = 1000)
        {
            if (pools.ContainsKey(_prefabToPool)) return pools[_prefabToPool];

            var obj = new GameObject($"{_prefabToPool.name} Pool");
            obj.transform.parent = transform;
            var pool = obj.AddComponent<PoolInstance>();
            pool.prefab = _prefabToPool;
            pool.defaultCapacity = _defaultCapacity;
            pool.maxSize = _maxSize;

            pools.Add(_prefabToPool, pool);
            return pool;
        }

    }
}