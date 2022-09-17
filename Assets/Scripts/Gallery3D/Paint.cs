using System;
using System.Collections.Generic;
using System.IO;
using Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Windows.WebCam;

namespace Gallery3D
{
    public enum PaintDir
    {
        Vert = 0,
        Horz,
    }

    public class Paint : ArtBase
    {
        [SerializeField] private List<GameObject> paints;

        public override void UpdateArt(FileInfo pFile)
        {
            // Init the paints
            foreach (GameObject pPaint in paints)
                pPaint.SetActive(false);
            // Load the file structure
            _pTexture = ImageFunctions.LoadTexture(pFile);
            _strTag = pFile.Name;
            _strAccessPath = pFile.FullName;
            // Select the frame based on art size
            GameObject pObject;
            pObject = (_pTexture.width > _pTexture.height)
                ? paints[(int) PaintDir.Horz]
                : paints[(int) PaintDir.Vert];
            pObject.SetActive(true);
            // Set-up the art texture
            Renderer pRender = pObject.GetComponent<Renderer>();
            pRender.materials[1].mainTexture = _pTexture;
        }

        public override void UpdateArt(string strImageEncoded)
        {
            // Init the paints
            foreach (GameObject pPaint in paints)
                pPaint.SetActive(false);
            // Decode the image from string
            _pTexture = ImageFunctions.DecodeTexture(strImageEncoded);
            _strTag = "";
            _strAccessPath = strImageEncoded;
            // Select the frame based an art size
            GameObject pObject;
            pObject = (_pTexture.width > _pTexture.height)
                ? paints[(int) PaintDir.Horz]
                : paints[(int) PaintDir.Vert];
            pObject.SetActive(true);
            // Set-up the art texture
            Renderer pRender = pObject.GetComponent<Renderer>();
            pRender.materials[1].mainTexture = _pTexture;
        }

        public void OnTriggerEnter(Collider other)
        {
            BoxCollider pCollider = GetComponent<BoxCollider>();
            pCollider.isTrigger = false;
            OnDeliverTexture(this, new TextureArgs(_pTexture, _strTag, _strAccessPath));
        }

        public void OnTriggerExit(Collider other)
        {
            BoxCollider pCollider = GetComponent<BoxCollider>();
            pCollider.isTrigger = true;
        }
    }
}