using System.IO;
using Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CoverFlow2D
{
    public class Cover : ArtBase, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField] private Image imageBackground;
        [SerializeField] private Image imageCover;
        private readonly float _fMaxWidth = 480f;
        private readonly float _fMaxHeight = 480f;
        private readonly Color _pDefaultColor = new Color(0f, 0f, 0f, 0f);
        private readonly Color _pHighlightColor = Color.magenta;
        
        public override void UpdateArt(FileInfo pFile)
        {
            imageBackground.color = _pDefaultColor;
            ImageFunctions.LoadImage(ref imageCover, pFile, _fMaxWidth, _fMaxHeight);
            // Load the file structure
            _pTexture = imageCover.sprite.texture;
            _strTag = pFile.Name;
            _strAccessPath = pFile.FullName;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            imageBackground.color = _pHighlightColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnDeliverTexture(this, new TextureArgs(_pTexture, _strTag, _strAccessPath));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            imageBackground.color = _pDefaultColor;
        }
    }
}