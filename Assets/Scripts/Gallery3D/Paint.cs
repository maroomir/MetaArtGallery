using System;
using System.Collections.Generic;
using System.IO;
using Common;
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
        public event SendImageCallback OnImageSendEvent;
            
        [SerializeField] private List<GameObject> paints;

        public Texture Texture { get; private set; }
        public string Tag { get; private set; }
        public string AccessPath { get; private set; }

        public void UpdateArt(FileInfo pFile)
        {
            // Init the paints
            foreach (GameObject pPaint in paints)
                pPaint.SetActive(false);
            // Load the file structure
            Texture = ImageFactory.LoadTexture(pFile);
            Tag = pFile.Name;
            AccessPath = pFile.FullName;
            // Select the frame based on art size
            GameObject pObject;
            pObject = (Texture.width > Texture.height)
                ? paints[(int) PaintDir.Horz]
                : paints[(int) PaintDir.Vert];
            pObject.SetActive(true);
            // Set-up the art texture
            Renderer pRender = pObject.GetComponent<Renderer>();
            pRender.materials[1].mainTexture = Texture;
        }

        public void OnTriggerEnter(Collider other)
        {
            BoxCollider pCollider = GetComponent<BoxCollider>();
            pCollider.isTrigger = false;
            OnImageSendEvent?.Invoke(this, new TextureArgs(Texture, Tag, AccessPath));
        }

        public void OnTriggerExit(Collider other)
        {
            BoxCollider pCollider = GetComponent<BoxCollider>();
            pCollider.isTrigger = true;
        }
    }
}