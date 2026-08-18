using System;
using UnityEngine;

namespace Dylanng
{
    public class VfxManager : ManagerBase
    {
        [SerializeField] private VfxLibrarySO library;
        
        public override void Initialize()
        {
            base.Initialize();
            ServiceLocator.Register<VfxManager>(this);
        }

        protected override void OnGameBooted(IGameBootedEvent evt)
        {
            base.OnGameBooted(evt);
            PoolManager poolManager = ServiceLocator.Get<PoolManager>();

            foreach (VfxData vfxData in library.Library)
            {
                if (vfxData.VFX != null)
                {
                    poolManager.AddCreatePoolAction(vfxData.Type.ToString(), vfxData.VFX, 1);
                }
                else
                {
                    Debug.LogWarning($"[VfxManager] VFX Prefab for key '{vfxData.Type}' is null in library '{library.name}'!");
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ServiceLocator.Unregister<VfxManager>();
        }

        public void PlayVFX(VfxKey key, Vector3 position, IVfxPayload payload = null, Action onComplete = null)
        {
            var vfx = ServiceLocator.Get<PoolManager>()?.Spawn<BaseVFX>(key.ToString(), position, Quaternion.identity);
            if (vfx != null)
            {
                vfx.Setup(payload);
                vfx.Play(position, 
                    (finishedVfx) => 
                    {
                        ReturnToPool(key, finishedVfx);
                        onComplete?.Invoke();
                    });
            }
        }

        public void PlayVFX(VfxKey key, Transform parent, IVfxPayload payload = null)
        {
            var vfx = ServiceLocator.Get<PoolManager>()?.Spawn<BaseVFX>(key.ToString(), parent.position, Quaternion.identity);
            vfx.transform.SetParent(parent);
            if (vfx != null)
            {
                vfx.Setup(payload);
                vfx.Play(parent.position, null);
            }
        }

        private void ReturnToPool(VfxKey key, BaseVFX vfx)
        {
            ServiceLocator.Get<PoolManager>()?.Despawn(key.ToString(), vfx);
        }
    }
}