
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
        void ShowWithAnimation(Action complete = null);
        void Hide();
        void HideWithAnimation(Action complete = null);
        bool IsShow();
        void Lock(bool value);
        UiView GetUiView();
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


        private T _view;
        public T View => _view;
        public UiView GetUiView() => _view;


        protected UiPresenter(T view)
        {
            _view = view;
            _onShow = new Subject<UiPresenter>();
            _onHide = new Subject<UiPresenter>();
        }
        

        public void Lock(bool value)
        {
            foreach (var button in _view.GetComponentsInChildren<Button>())
                button.enabled = !value;
        }

        public virtual void Show()
        {
            View.Show();
            _onShow.OnNext(this);
        }

        public virtual void Hide()
        {
            View.Hide();
            _onHide.OnNext(this);
        }

        public bool IsShow()
        {
            return View.IsShow();
        }

        public virtual void HideWithAnimation(Action complete = null)
        {
            Hide();
            complete?.Invoke();
        }
        
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