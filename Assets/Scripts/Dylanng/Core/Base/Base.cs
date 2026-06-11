using TMPro;
using UnityEngine;

namespace Dylanng.Core.Base
{
    public class MonoBehaviourBase : MonoBehaviour
    {
        protected virtual void Awake() { }
        protected virtual void Start() { }
        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
        protected virtual void OnDestroy() { }

        // --- API ---
        private Transform _transformCache;
        protected Transform TransformCache
        {
            get
            {
                if (_transformCache == null) _transformCache = transform;
                return _transformCache;
            }
        }

        private GameObject _gameObjectCache;
        protected GameObject GameObjectCache
        {
            get
            {
                if(_gameObjectCache == null) _gameObjectCache = gameObject;
                return _gameObjectCache;
            }
        }

        public Transform Parent { get; private set; }
        public Transform RootParent { get; private set; }


        #region --- ACTIVE ---
        public bool IsActive() => GameObjectCache.activeInHierarchy;

        public virtual void SetActive(bool isActive)
        {
            GameObjectCache.SetActive(isActive);
        }
        #endregion

        public virtual void SetParent(Transform parent, bool isPosRotReseted = false)
        {
            TransformCache.SetParent(parent);
            Parent = parent;
            RootParent = parent.root;

            if (isPosRotReseted)
            {
                TransformCache.localPosition = Vector3.zero;
                TransformCache.localRotation = Quaternion.identity;
            }
        }

        #region --- POSITION ---
        public virtual void SetWorldPosition(Vector3 targetPosition)
        {
            TransformCache.position = targetPosition;
        }

        public virtual void SetLocalPosition(Vector3 targetPosition)
        {
            TransformCache.localPosition = targetPosition;
        }
        #endregion

        #region --- ROTATION ---
        public virtual void SetWorldRotation(Quaternion targetRotation)
        {
            TransformCache.rotation = targetRotation;
        }

        public virtual void SetLocalRotation(Quaternion targetRotation)
        {
            TransformCache.localRotation = targetRotation;
        }
        #endregion

        #region --- SCALE ---
        public virtual void SetWorldScale(Vector3 targetScale)
        {
            if (Parent != null)
            {
                TransformCache.SetParent(null);
            }

            TransformCache.localScale = targetScale;

            if (Parent != null)
            {
                TransformCache.SetParent(Parent);
            }
        }

        public virtual void SetLocalScale(Vector3 targetScale)
        {
            TransformCache.localScale = targetScale;
        }
        #endregion


    }

    public abstract class ManagerBase : MonoBehaviourBase, IManager
    {
        public abstract void Initialize();
    }

    public abstract class ServiceBase : IService
    {
        public abstract void Initialize();
        public virtual void Dispose() { }
    }

    public abstract class SystemBase : ISystem
    {
        public abstract void Initialize();
    }
    public abstract class ControllerBase : MonoBehaviourBase { }
    public abstract class ComponentBase : MonoBehaviourBase { }

    public abstract class CommandBase
    {
        public abstract void Execute(); 
        public abstract void Undo();
    }

    public abstract class FactoryBase<T>
    {
        public abstract T Create();
    }
    public abstract class ScriptableData : ScriptableObject { }
}
