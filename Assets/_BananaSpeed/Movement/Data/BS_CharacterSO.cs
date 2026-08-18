using _BananaSpeed.Movement.Data;
using UnityEngine;

namespace _BananaSpeed.Movement.Data
{
    [CreateAssetMenu(menuName = "BananaSpeed/CharacterSO")]
    public class BS_CharacterSO : ScriptableObject
    {
        [field: SerializeField] public BS_GroundData Ground { get; private set; }
        [field: SerializeField] public BS_AirborneData Airborne { get; private set; }
        [field: SerializeField] public BS_PlayerAnimationData AnimationData { get; private set; }
    }
}
