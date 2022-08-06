using System;
using System.IO;
using CoverFlow2D;
using UnityEngine;

namespace Gallery3D
{
    public class PaintGenerator : MonoBehaviour
    {
        [SerializeField] private new string tag = "Painters";

        private void Awake()
        {
            Application.runInBackground = true;
            string strResourcePath = Path.Combine(Application.dataPath, "Resources");

            foreach (GameObject pObject in GameObject.FindGameObjectsWithTag(tag))
            {
                PaintSpawner pSpawner = pObject.GetComponent<PaintSpawner>();
                pSpawner.Init();
                pSpawner.UpdateArts(new DirectoryInfo(strResourcePath));
            }
        }
    }
}