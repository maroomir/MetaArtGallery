using System;
using System.Collections.Generic;
using Common;
using UnityEditor;
using UnityEngine;

namespace CoverFlow2D
{
    public class CollectionView : MonoBehaviour
    {
        private void Awake()
        {
            Application.runInBackground = true;
        }

        private void Update()
        {
            UpdateKeyInput();
        }

        private void UpdateKeyInput()
        {
            // Observe the key operations
            KeyCode[] pObserveKeys = new[] {KeyCode.LeftArrow, KeyCode.RightArrow};
            KeyCode pResultKey = KeyCodeFactory.GetInputKeyDown(pObserveKeys);
            switch (pResultKey)
            {
                case KeyCode.LeftArrow:
                    Debug.Log("Step the left");
                    break;
                case KeyCode.RightArrow:
                    Debug.Log("Step the right");
                    break;
            }
        }
    }
}