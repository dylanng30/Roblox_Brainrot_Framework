using Dylanng;
using UnityEngine;

namespace _BananaSpeed.Builder
{
    public class BS_Block : PoolableObject
    {
        [SerializeField] private Renderer renderer;

        private MaterialPropertyBlock _mpb;
        
        private static readonly int MainTexId = Shader.PropertyToID("_MainTexture");
        
        protected override void Awake()
        {
            base.Awake();
            
            _mpb = new MaterialPropertyBlock();
        }

        

        public void SetTexture(Texture2D texture)
        {
            renderer.GetPropertyBlock(_mpb);
            _mpb.SetTexture(MainTexId, texture);
            renderer.SetPropertyBlock(_mpb);
        }
    }
}