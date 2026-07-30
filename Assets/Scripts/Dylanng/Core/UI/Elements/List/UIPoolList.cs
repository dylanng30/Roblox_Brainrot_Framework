using System.Collections.Generic;
using UnityEngine;

namespace Dylanng.Core.UI.Elements.List
{
    public interface IPoolItem<T>
    {
        void Setup(T data);
    }

    public class UIPoolList<T, TPrefab> : MonoBehaviour where TPrefab : MonoBehaviour, IPoolItem<T>
    {
        [SerializeField] private TPrefab itemPrefab;
        [SerializeField] private Transform contentContainer;
        
        private Queue<TPrefab> _pool = new Queue<TPrefab>();
        private List<TPrefab> _activeItems = new List<TPrefab>();

        public void SetData(IEnumerable<T> itemsData)
        {
            Clear();

            if (itemsData == null) return;

            foreach (var data in itemsData)
            {
                var item = GetItem();
                item.Setup(data);
                _activeItems.Add(item);
            }
        }

        public void Clear()
        {
            foreach (var item in _activeItems)
            {
                item.gameObject.SetActive(false);
                _pool.Enqueue(item);
            }
            _activeItems.Clear();
        }

        private TPrefab GetItem()
        {
            if (_pool.Count > 0)
            {
                var item = _pool.Dequeue();
                item.gameObject.SetActive(true);
                item.transform.SetAsLastSibling();
                return item;
            }
            else
            {
                var item = Instantiate(itemPrefab, contentContainer);
                return item;
            }
        }
        
        public List<TPrefab> GetActiveItems() => _activeItems;
    }
}
