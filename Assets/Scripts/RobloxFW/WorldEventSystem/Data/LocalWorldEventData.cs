using Dylanng.Core.Base;
using UnityEngine;

namespace RobloxFW.WorldEventSystem.Data
{
    [CreateAssetMenu(fileName = "NewWorldEventData", menuName = "RobloxFW/EventSystem/NewWorldEventData")]
    public class LocalWorldEventData : ScriptableData
    {
        public WorldEventEnum EventId;
        public Texture2D[] EnviromentTextures;
    }
}

