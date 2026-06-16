using RobloxFW.MovementSystem.Data.Player.States.Airborne;
using RobloxFW.MovementSystem.Data.Player.States.Grounded;
using UnityEngine;

namespace RobloxFW.MovementSystem.Data.Player.SO
{
    [CreateAssetMenu(fileName = "PlayerMovement", menuName = "RobloxFW/PlayerMovementSO")]
    public class PlayerMovementSO : ScriptableObject
    {
        [field: SerializeField] public PlayerGroundedData GroundedData { get; private set; }
        [field: Space(10)]
        [field: SerializeField] public PlayerAirborneData AirborneData { get; private set; }
    }
} 