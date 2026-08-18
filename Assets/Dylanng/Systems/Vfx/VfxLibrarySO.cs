using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dylanng
{
    [CreateAssetMenu(fileName = "NewVfxLibrary", menuName = "Dylanng/VfxLibrary")]
    public class VfxLibrarySO : ScriptableObject
    {
        public List<VfxData> Library = new List<VfxData>();
    }

    [Serializable]
    public class VfxData
    {
        public VfxKey Type;
        public BaseVFX VFX;
    }
}