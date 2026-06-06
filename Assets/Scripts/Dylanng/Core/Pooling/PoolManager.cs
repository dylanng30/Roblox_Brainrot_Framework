using System.Collections.Generic;
using Dylanng.Core.Base;
using Dylanng.Core.Systems.TickSystem;
using UnityEngine;

namespace Dylanng.Core.Pooling
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
            _poolDictionary = new Dictionary<string, Queue<PoolableObject>>();
            _prefabs = new Dictionary<string, PoolableObject>();
            _instantiationQueue = new Queue<(string, PoolableObject)>();
            
            _poolRoot = new GameObject("[Pool]").transform;
            DontDestroyOnLoad(_poolRoot.gameObject);
            ServiceLocator.Register<PoolManager>(this);
        }
        
        public void AddCreatePoolAction(string poolKey, PoolableObject prefab, int amount)
        {
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
            obj.gameObject.SetActive(false);
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

            var obj = _poolDictionary[poolKey].Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.OnSpawn();
            return obj as T;
        }

        public void Despawn(string poolKey, PoolableObject obj)
        {
            obj.OnDespawn();
            obj.transform.SetParent(_poolRoot);
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
