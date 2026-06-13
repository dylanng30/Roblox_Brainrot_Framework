using System.Collections;
using System.Collections.Generic;
using Dylanng.Core.Base;
using RobloxFW.WorldEventSystem.Interfaces;
using UnityEngine;

namespace RobloxFW.WorldEventSystem.Manager
{
    public class EventManager : ManagerBase
    {
        [SerializeField] private Material eventMaterial;

        private Dictionary<int, IWorldEventStrategy> _worldEventStrategiesDic;

        public override void Initialize()
        {
            _worldEventStrategiesDic = new Dictionary<int, IWorldEventStrategy>();

        }
    }
}
