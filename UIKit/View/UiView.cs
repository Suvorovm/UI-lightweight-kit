using CGK.Utils;
using UnityEngine;

namespace UiKit.View
{
    [RequireComponent(typeof(Canvas))]
    public class UiView : UiElement
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
    }
}