using System.Collections.Generic;
using UiKit.View;
using UnityEngine;

namespace UiKit.Presenter
{
    public abstract class UiCollection<TView> : MonoBehaviour where TView : UiView
    {
        [SerializeField] private Transform _collectionRoot;

        [SerializeField] private TView _collectionPrefab;

        private readonly List<TView> _activeItems = new List<TView>();
        private readonly List<TView> _inactiveItems = new List<TView>();

        public TView GetCollectionPrefab => _collectionPrefab;
        public Transform GetCollectionRoot => _collectionRoot;

        public TView AddItem()
        {
            if (_inactiveItems.Count > 0)
            {
                return ProcessReuse();
            }
            else
            {
                return ProcessCreation();
            }

            TView ProcessReuse()
            {
                TView itemToActivate = _inactiveItems[0];
                ActivateItem(itemToActivate);
                return itemToActivate;
            }

            TView ProcessCreation()
            {
                TView item = Instantiate(_collectionPrefab, _collectionRoot);


                if (!item.gameObject.activeInHierarchy)
                    item.gameObject.SetActive(true);

                _activeItems.Add(item);

                return item;
            }
        }


        public void ActivateItem(TView item)
        {
            item.gameObject.SetActive(true);
            _inactiveItems.Remove(item);
            _activeItems.Add(item);
        }


        public void DeactivateItem(TView item)
        {
            item.gameObject.SetActive(false);
            _activeItems.Remove(item);
            _inactiveItems.Add(item);
        }


        public void RemoveItem(TView item)
        {
            _activeItems.Remove(item);
            Destroy(item.gameObject);
        }

        public List<TView> GetItems()
        {
            return _activeItems;
        }

        public void Clear()
        {
            foreach (var item in _activeItems)
            {
                Destroy(item.gameObject);
            }

            _activeItems.Clear();
        }

        public int Count()
        {
            return _activeItems.Count;
        }
    }
}