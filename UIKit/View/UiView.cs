using CGK.Utils;
using UnityEngine;

namespace UiKit.View
{
    [RequireComponent(typeof(Canvas))]
    public class UiView : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private Canvas _viewCanvas;

        public Canvas ViewCanvas => _viewCanvas;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _viewCanvas = GetComponent<Canvas>();
        }
#endif
        public void Show(bool isShow = true)
        {
            gameObject.SetActive(isShow);
        }

        public void Hide()
        {
            if (IsShow())
                gameObject.SetActive(false);
        }

        public bool IsShow()
        {
            return gameObject.activeSelf;
        }
    }
}