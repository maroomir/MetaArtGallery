using System.Diagnostics.Tracing;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CoverFlow2D
{
    public class Cover : MonoBehaviour, IArt
    {
        [SerializeField] private Image imageBackground;
        [SerializeField] private Image imageCover;
        private readonly float _fMaxWidth = 480f;
        private readonly float _fMaxHeight = 480f;
        private readonly Color _pDefaultColor = new Color(0f, 0f, 0f, 0f);
        private readonly Color _pHighlightColor = Color.magenta;

        public void UpdateArt(FileInfo pFile)
        {
            imageBackground.color = _pDefaultColor;
            ImageFactory.LoadImage(ref imageCover, pFile, _fMaxWidth, _fMaxHeight);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            imageBackground.color = _pHighlightColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            imageBackground.color = _pDefaultColor;
        }
    }
}