using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dylanng
{
    public class ParticleVFX : BaseVFX
    {
        [SerializeField] private List<ParticleSystem> particleSystems;

        public override void Play(Vector3 position, Action<BaseVFX> onComplete)
        {
            base.Play(position, onComplete);

            if (particleSystems != null)
            {
                foreach (var ps in particleSystems)
                {
                    ps.Play();
                    StartCoroutine(WaitAndReturn(ps.main.duration));
                }
            }
        }

        public override void Stop()
        {
            if (particleSystems != null)
            {
                foreach (var ps in particleSystems)
                {
                    ps.Stop();
                }
            }

            OnDespawn();
        }

        private IEnumerator WaitAndReturn(float duration)
        {
            yield return new WaitForSeconds(duration);
            ReturnToPool();
        }
    }
}