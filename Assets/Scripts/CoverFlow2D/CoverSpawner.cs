using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CoverFlow2D
{
    public class CoverSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject panelCover;
        [SerializeField] private Scrollbar horizontalScrollbar;
        [SerializeField] private Transform parentTransform;

        
        private List<Cover> _pListCovers;

        public void Init()
        {
            _pListCovers ??= new List<Cover>();
            foreach (Cover pCover in _pListCovers)
            {
                Destroy(pCover.gameObject);
            }

            _pListCovers?.Clear();
        }

        public void UpdateCovers(DirectoryInfo pDirectory)
        {
            foreach (DirectoryInfo pDir in pDirectory.GetDirectories())
                UpdateCovers(pDir);
            foreach (FileInfo pFile in pDirectory.GetImages())
                SpawnCover(pFile);
        }

        private void SpawnCover(FileInfo pFile)
        {
            GameObject pClone = Instantiate(panelCover);
            pClone.transform.SetParent(parentTransform);
            pClone.transform.localScale = Vector3.one;

            Cover pCover = pClone.GetComponent<Cover>();
            pCover.Init(pFile);
            _pListCovers.Add(pCover);
        }
    }
}
