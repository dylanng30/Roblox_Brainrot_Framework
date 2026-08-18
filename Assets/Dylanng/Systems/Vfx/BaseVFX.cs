using System;
using UnityEngine;

namespace Dylanng
{
    public abstract class BaseVFX : PoolableObject
    {
        protected Action<BaseVFX> _onCompleteCallback;

        public virtual void Setup(IVfxPayload payload)
        {

        }

        public virtual void Play(Vector3 position, Action<BaseVFX> onComplete)
        {
            transform.position = position;
            _onCompleteCallback = onComplete;
        }

        public virtual void Play(Vector3 spawnPos, Vector3 endPos, Action<BaseVFX> onComplete)
        {

        }

        public virtual void Stop()
        {
            ReturnToPool();
        }

        protected void ReturnToPool()
        {
            _onCompleteCallback?.Invoke(this);
        }
    }
}