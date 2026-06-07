using System;
using UnityEngine;

namespace RobloxFW.MovementSystem.Data.Player
{
    [Serializable]
    public class PlayerRotationData
    {
        [field: SerializeField] public Vector3 TargetRotationReachTime { get; private set; }
    }
}