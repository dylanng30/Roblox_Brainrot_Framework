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
