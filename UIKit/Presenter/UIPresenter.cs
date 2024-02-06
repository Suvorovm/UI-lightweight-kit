using System;
using UiKit.View;
using UniRx;

namespace UiKit.Presenter
{
    public interface IUiPresenter
    {
        IObservable<UiPresenter> OnShow { get; }
        IObservable<UiPresenter> OnHide { get; }
        void Show();
        void Show(params object[] showParameters);
        void Hide();
        bool IsShow();
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

        private T _uiView;
        public T UiView => _uiView;
        public UiView GetUiView() => _uiView;


        protected UiPresenter(T uiView)
        {
            _uiView = uiView;
            _onShow = new Subject<UiPresenter>();
            _onHide = new Subject<UiPresenter>();
        }
        
        public virtual void Show()
        {
            UiView.Show();
            _onShow.OnNext(this);
        }

        public virtual void Show(params object[] showParameters)
        {
            Show();
        }

        public virtual void Hide()
        {
            UiView.Hide();
            _onHide.OnNext(this);
        }

        public bool IsShow()
        {
            return UiView.IsShow();
        }
        
        public override void Dispose()
        {
            Disposables?.Dispose();
        }
    }
}