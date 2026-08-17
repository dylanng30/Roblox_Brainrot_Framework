using System;
using UnityEngine;

namespace Dylanng
{
    [Serializable]
    public struct SceneSlotData
    {
        public string SlotKey;
        public string SceneName;
    }

    [CreateAssetMenu(fileName = "NewSceneNavigationSO", menuName = "Dylanng/Scene/Navigation")]
    public class SceneNavigationSO : ScriptableObject
    {
        public SceneSlotData ActiveScene;
        public SceneSlotData[] AdditiveScenes;
        public string[] UnloadSceneSlots; 
        public bool ClearUnusedAssets;
        public bool HasOverlay;
    }
}