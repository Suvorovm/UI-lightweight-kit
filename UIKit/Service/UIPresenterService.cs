using System.Collections.Generic;
using System.Linq;
using CGK.Utils;
using JetBrains.Annotations;
using UIKit.Factory;
using UIKit.Presenter;
using UIKit.View;
using UniRx;
using UnityEngine;

namespace UIKit.Service
{
    public class UIPresenterService
    {
        private readonly IUIPresenterFactory _uiPresenterFactory;
        private readonly List<PresenterData> _showedPresenters;
        
        private GameObject _uiRoot;

        private int _maxSortingOrder;

        public UIPresenterService(IUIPresenterFactory uiPresenterFactory)
        {
            _uiPresenterFactory = uiPresenterFactory;
            _showedPresenters = new List<PresenterData>();
        }

        [PublicAPI]
        public void Init(GameObject root)
        {
            _uiRoot = root;
            ValidateUIStruct();
        }

        public IUiPresenter ShowPresenter<T>()
            where T : IUiPresenter

        {
            var data = TryGetPresenter<T>();
            if (data != null)
            {
                PushUpExistingPresenter(data);
                return data.Presenter;
            }
            T presenter = _uiPresenterFactory.GetPresenter<T>();
            presenter.Show();
            _maxSortingOrder++;
            presenter.GetUIView().ViewCanvas.overrideSorting = true;
            presenter.GetUIView().ViewCanvas.sortingOrder = _maxSortingOrder;
            CompositeDisposable compositeDisposable = new CompositeDisposable();
            presenter.OnHide
                .Subscribe(_ =>
                {
                    HidePresenter<T>();
                })
                .AddTo(compositeDisposable);
            _showedPresenters.Add(new PresenterData()
            {
                Order = _maxSortingOrder,
                Presenter = presenter,
                UiView = presenter.GetUIView(),
                HideDisposable = compositeDisposable
            });
            return presenter;
        }

        public void HidePresenter<T>()
            where T : IUiPresenter
        {
            if (_showedPresenters.Count == 0)
            {
                return;
            }

            PresenterData presenterData = TryGetPresenter<T>();
            presenterData.HideDisposable?.Dispose();
            presenterData.HideDisposable = null;
            RemoveDialog(presenterData);
        }
        private void RemoveDialog(PresenterData removingPresenterData)
        {
            for (int i = 0; i < _showedPresenters.Count; i++)
            {
                if (_showedPresenters[i].Order > removingPresenterData.Order)
                {
                    _showedPresenters[i].Order--;
                    _showedPresenters[i].UiView.ViewCanvas.sortingOrder--;
                }
            }

            _maxSortingOrder--;
            removingPresenterData.UiView.Hide();
            _showedPresenters.Remove(removingPresenterData);

        }

        public bool IsPresenterShowed<T>()
            where T : IUiPresenter
        {
            PresenterData presenterData = TryGetPresenter<T>();
            if (presenterData == null)
            {
                return false;
            }

            return true;
        }

        private PresenterData TryGetPresenter<T>()
            where T : IUiPresenter

        {
            PresenterData presenterData = _showedPresenters.FirstOrDefault(p => p.Presenter is T);
            return presenterData;
        }

        private void ValidateUIStruct()
        {
            Predications.CheckNotNull(_uiRoot);
        }
        private void PushUpExistingPresenter(PresenterData existingPresenter)
        {
            for (int i = 0; i < _showedPresenters.Count; i++)
            {
                if (_showedPresenters[i].Order > existingPresenter.Order)
                {
                    _showedPresenters[i].Order--;
                    _showedPresenters[i].UiView.ViewCanvas.sortingOrder--;
                }
            }

            existingPresenter.Order = _maxSortingOrder;
            existingPresenter.UiView.ViewCanvas.sortingOrder = _maxSortingOrder;
        }

        private record PresenterData
        {
            public UiView UiView;
            public IUiPresenter Presenter;
            public int Order;
            public CompositeDisposable HideDisposable;
        }
    }
}