using System.Diagnostics.Tracing;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CoverFlow2D
{
    public class Cover : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField] private Image imageBackground;
        [SerializeField] private Image imageCover;
        private readonly float _fMaxWidth = 480f;
        private readonly float _fMaxHeight = 480f;
        private readonly Color _pDefaultColor = new Color(0f, 0f, 0f, 0f);
        private readonly Color _pHighlightColor = Color.magenta;

        public void Init(FileInfo pFile)
        {
            imageBackground.color = _pDefaultColor;

            byte[] pBytes = File.ReadAllBytes(pFile.FullName);
            Texture2D pTexture = new Texture2D(0, 0);
            if (pBytes.Length > 0)
                pTexture.LoadImage(pBytes);
            if (pTexture.width > _fMaxWidth)
                imageCover.rectTransform.sizeDelta =
                    new Vector2(_fMaxWidth, _fMaxWidth / pTexture.width * pTexture.height);
            else if (pTexture.height > _fMaxHeight)
                imageCover.rectTransform.sizeDelta =
                    new Vector2(_fMaxHeight / pTexture.height * pTexture.width, _fMaxHeight);
            else
                imageCover.rectTransform.sizeDelta = new Vector2(pTexture.width, pTexture.height);
            imageCover.sprite =
                Sprite.Create(pTexture, new Rect(0, 0, pTexture.width, pTexture.height), new Vector2(0.5f, 0.5f));
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