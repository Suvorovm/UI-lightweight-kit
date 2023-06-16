using System;
using UnityEngine;

namespace UIKit.View
{
    [RequireComponent(typeof(Canvas))]
    public class UiView : MonoBehaviour
    {
        public Canvas ViewCanvas { get; private set; }
        

        private void Awake()
        {
            ViewCanvas = GetComponent<Canvas>();
        }

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