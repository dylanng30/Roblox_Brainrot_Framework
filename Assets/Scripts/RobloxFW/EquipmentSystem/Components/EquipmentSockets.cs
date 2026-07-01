using System.Collections.Generic;
using UnityEngine;
using RobloxFW.EquipmentSystem.Core;

namespace RobloxFW.EquipmentSystem.Components
{
    public class EquipmentSockets : MonoBehaviour
    {
        [System.Serializable]
        public struct SocketMapping
        {
            public EquipmentSlot Slot;
            public Transform Transform;
        }

        [SerializeField] private List<SocketMapping> _sockets = new List<SocketMapping>();
        private Dictionary<EquipmentSlot, Transform> _socketDict;

        private void Awake()
        {
            _socketDict = new Dictionary<EquipmentSlot, Transform>();
            foreach (var mapping in _sockets)
            {
                if (!_socketDict.ContainsKey(mapping.Slot))
                {
                    _socketDict.Add(mapping.Slot, mapping.Transform);
                }
            }
        }

        public Transform GetSocket(EquipmentSlot slot)
        {
            if (_socketDict != null && _socketDict.TryGetValue(slot, out Transform t))
            {
                return t;
            }
            Debug.LogWarning($"[EquipmentSockets] Socket for {slot} not found on {gameObject.name}");
            return null;
        }
    }
}
