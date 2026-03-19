using UnityEngine;

namespace UiKit.View
{
    public class UiElement : MonoBehaviour
    {
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
