using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dylanng
{
    [CreateAssetMenu(menuName = "Dylanng/SoundLibrary")]
    public class SoundLibrarySO : ScriptableObject
    {
        public SoundData[] Sounds;
    }
}