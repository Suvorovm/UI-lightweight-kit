using UIKit.Presenter;

namespace UIKit.Factory
{
    public interface IUIPresenterFactory
    {
        T GetPresenter<T>()
            where T : IUiPresenter;
    }
}