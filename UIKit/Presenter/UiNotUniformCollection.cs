using System.Collections.Generic;
using UiKit.View;
using UnityEngine;

namespace UiKit.Presenter
{
    public class UINotUniformCollection : MonoBehaviour
    {
        [SerializeField]
        private Transform _collectionRoot;
        
        private readonly List<UiElement> _items = new List<UiElement>();

        public TView AddItem<TView>(TView prefab) where TView : UiElement
        {
            var item = Instantiate(prefab, _collectionRoot);

            if (!item.gameObject.activeInHierarchy)
                item.gameObject.SetActive(true);

            _items.Add(item);
            return item;
        }

        public void RemoveItem<TView>(TView item) where TView : UiElement
        {
            _items.Remove(item);
            Destroy(item.gameObject);
        }

        public List<UiElement> GetItems()
        {
            return _items;
        }

        public void Clear()
        {
            foreach (var item in _items)
                Destroy(item.gameObject);

            _items.Clear();
        }

        public int Count()
        {
            return _items.Count;
        }
    }
}