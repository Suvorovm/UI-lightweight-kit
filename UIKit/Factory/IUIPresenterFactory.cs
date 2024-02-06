using UiKit.Presenter;

namespace UiKit.Factory
{
    public interface IUiPresenterFactory
    {
        T GetPresenter<T>()
            where T : IUiPresenter;
    }
}