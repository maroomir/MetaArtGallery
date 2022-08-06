using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gallery3D
{
    public enum PaintDir
    {
        Vert = 0,
        Horz,
    }

    public class Paint : MonoBehaviour, IArt
    {
        [SerializeField] private List<GameObject> paints;
        private Texture _pTexture;

        public void UpdateArt(FileInfo pFile)
        {
            foreach (GameObject pPaint in paints)
                pPaint.SetActive(false);

            GameObject pObject;
            _pTexture = ImageFactory.LoadTexture(pFile);
            pObject = (_pTexture.width > _pTexture.height)
                ? paints[(int) PaintDir.Horz]
                : paints[(int) PaintDir.Vert];
            pObject.SetActive(true);
            Renderer pRender = pObject.GetComponent<Renderer>();
            pRender.materials[1].mainTexture = _pTexture;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //
        }
    }
}