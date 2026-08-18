using System.Collections.Generic;
using UnityEngine;

namespace Dylanng
{
    public class PoolManager : ManagerBase, IUpdatable
    {
        [SerializeField] private int maxInstantiatesPerFrame = 2;
        
        private Dictionary<string, Queue<PoolableObject>> _poolDictionary;
        private Dictionary<string, PoolableObject> _prefabs;
        private Queue<(string poolKey, PoolableObject prefab)> _instantiationQueue;
        
        private Transform _poolRoot;

        public override void Initialize()
        {
            base.Initialize();
            Debug.Log("Initializing Pool Manager");
            _poolDictionary = new Dictionary<string, Queue<PoolableObject>>();
            _prefabs = new Dictionary<string, PoolableObject>();
            _instantiationQueue = new Queue<(string, PoolableObject)>();
            
            _poolRoot = new GameObject("[Pool]").transform;
            //DontDestroyOnLoad(_poolRoot.gameObject);
            ServiceLocator.Register(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ServiceLocator.Unregister<PoolManager>();
        }

        public void ClearAllPools()
        {
            _poolDictionary.Clear();

        }

        public bool HasPool(string poolKey)
        {
            return _poolDictionary.ContainsKey(poolKey);
        }

        public void AddCreatePoolAction(string poolKey, PoolableObject prefab, int amount)
        {
            if (prefab == null)
            {
                Debug.LogError($"[PoolManager] Cannot create pool for key '{poolKey}' because the prefab is null!");
                return;
            }

            if (!_poolDictionary.ContainsKey(poolKey))
            {
                _poolDictionary.Add(poolKey, new Queue<PoolableObject>());
                _prefabs.Add(poolKey, prefab);
            }

            for (int i = 0; i < amount; i++)
            {
                _instantiationQueue.Enqueue((poolKey, prefab));
            }
        }
        
        public void OnUpdate(float deltaTime)
        {
            int instantiatesThisFrame = 0;
            
            while (_instantiationQueue.Count > 0 && instantiatesThisFrame < maxInstantiatesPerFrame)
            {
                (string poolKey, PoolableObject prefab) request = _instantiationQueue.Dequeue();
                CreateNewObject(request.poolKey, request.prefab);
                instantiatesThisFrame++;
            }
        }

        private PoolableObject CreateNewObject(string poolKey, PoolableObject prefab)
        {
            var obj = Instantiate(prefab, _poolRoot);
            obj.SetActive(false);
            _poolDictionary[poolKey].Enqueue(obj);
            return obj;
        }

        public T Spawn<T>(string poolKey, Vector3 position, Quaternion rotation) where T : PoolableObject
        {
            if (!_poolDictionary.ContainsKey(poolKey)) return null;

            if (_poolDictionary[poolKey].Count == 0)
            {
                CreateNewObject(poolKey, _prefabs[poolKey]);
            }

            PoolableObject obj = _poolDictionary[poolKey].Dequeue();
            obj.SetWorldPosition(position);
            obj.SetWorldRotation(rotation);
            obj.OnSpawn();
            return obj as T;
        }

        public void Despawn(string poolKey, PoolableObject obj)
        {
            obj.OnDespawn();
            obj.SetParent(_poolRoot, true);
            _poolDictionary[poolKey].Enqueue(obj);
        }
        
        public void ClearPool(string poolKey)
        {
            if (_poolDictionary.TryGetValue(poolKey, out Queue<PoolableObject> queue))
            {
                while (queue.Count > 0)
                {
                    var obj = queue.Dequeue();
                    if (obj != null) Destroy(obj.gameObject);
                }
                _poolDictionary.Remove(poolKey);
                _prefabs.Remove(poolKey);
            }
        }
    }
}
