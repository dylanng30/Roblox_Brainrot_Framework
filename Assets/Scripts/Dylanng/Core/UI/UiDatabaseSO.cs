using Dylanng.Core.Base;
using UnityEngine;

namespace Dylanng.Core.UI
{
    [CreateAssetMenu(fileName = "New UI Database", menuName = "Dylanng/UI Database")]
    public class UiDatabaseSO : ScriptableData
    {
        public UIBase[] UIs;
    }
}