using System.Collections.Generic;
using UnityEngine;

namespace RobloxFW.WorldEventSystem.Data.Local.WorldEventData
{
    [CreateAssetMenu(fileName = "NewLocalWorldEventDatabase", menuName = "RobloxFW/EventSystem/LocalWorldEventDatabase")]
    public class LocalWorldEventDatabaseSO : ScriptableObject
    {
        public List<WorldEventSO> LocalEvents = new List<WorldEventSO>();

        public WorldEventSO GetLocalData(WorldEventEnum eventId)
        {
            if (LocalEvents == null) return null;
            return LocalEvents.Find(e => e.EventId == eventId);
        }
    }
}
