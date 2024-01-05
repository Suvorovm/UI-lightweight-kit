
using System;
using UIKit.View;
using UniRx;
using UnityEngine.UI;

namespace UIKit.Presenter
{
    public interface IUiPresenter
    {
        IObservable<UiPresenter> OnShow { get; }
        IObservable<UiPresenter> OnHide { get; }
        void Show();
        void Show(params object[] showParameters);
        [Obsolete]
        void ShowWithAnimation(Action complete = null);
        void Hide();
        [Obsolete]
        void HideWithAnimation(Action complete = null);
        bool IsShow();
        void Lock(bool value);
        UiView GetUIView();
    }

    public abstract class UiPresenter : IDisposable
    {

        public virtual void Dispose()
        {
        }
        
    }

    public abstract class UiPresenter<T> : UiPresenter, IUiPresenter
        where T : UiView
    {
        private readonly Subject<UiPresenter> _onShow;
        private readonly Subject<UiPresenter> _onHide;
        protected readonly CompositeDisposable Disposables = new CompositeDisposable();
        public IObservable<UiPresenter> OnShow => _onShow;

        public IObservable<UiPresenter> OnHide => _onHide;


        private T _uiView;
        public T UIView => _uiView;
        public UiView GetUIView() => _uiView;


        protected UiPresenter(T uiView)
        {
            _uiView = uiView;
            _onShow = new Subject<UiPresenter>();
            _onHide = new Subject<UiPresenter>();
        }
        

        public void Lock(bool value)
        {
            foreach (var button in _uiView.GetComponentsInChildren<Button>())
                button.enabled = !value;
        }

        public virtual void Show()
        {
            UIView.Show();
            _onShow.OnNext(this);
        }

        public virtual void Show(params object[] showParameters)
        {
            Show();
        }

        public virtual void Hide()
        {
            UIView.Hide();
            _onHide.OnNext(this);
        }

        public bool IsShow()
        {
            return UIView.IsShow();
        }
        
        [Obsolete]
        public virtual void HideWithAnimation(Action complete = null)
        {
            Hide();
            complete?.Invoke();
        }
        
        [Obsolete]
        public virtual void ShowWithAnimation(Action complete = null)
        {
            Show();
            complete?.Invoke();
        }

        public override void Dispose()
        {
            Disposables?.Dispose();
        }
    }
}