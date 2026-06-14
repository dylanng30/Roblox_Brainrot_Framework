using Dylanng.Core.Base;
using UnityEngine;

namespace RobloxFW.WorldEventSystem.Data.Local.WorldEventData
{
    [CreateAssetMenu(fileName = "NewWorldEventSO", menuName = "RobloxFW/EventSystem/NewWorldEventSO")]
    public class WorldEventSO : ScriptableData
    {
        public WorldEventEnum EventId;
        public Texture2D[] EnviromentTextures;
    }
}

