using UnityEngine;

namespace Dylanng
{
    [CreateAssetMenu(fileName = "New UI Database", menuName = "Dylanng/UI Database")]
    public class UIDatabase : ScriptableObject
    {
        public UIBase[] UIs;
    }
}