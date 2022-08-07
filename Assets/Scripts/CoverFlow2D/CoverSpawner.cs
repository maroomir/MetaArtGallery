using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace CoverFlow2D
{
    public class CoverSpawner : SpawnerBase
    {
        [SerializeField] private GameObject panelCover;
        [SerializeField] private Scrollbar horizontalScrollbar;
        [SerializeField] private Transform parentTransform;
        
        private List<Cover> _pListCovers;

        public override void Init()
        {
            _pListCovers ??= new List<Cover>();
            foreach (Cover pCover in _pListCovers)
            {
                Destroy(pCover.gameObject);
            }

            _pListCovers?.Clear();
        }

        public override void SpawnArt(FileInfo pFile, int nIndex)
        {
            GameObject pClone = Instantiate(panelCover, parentTransform, true);
            pClone.transform.localScale = Vector3.one;

            Cover pCover = pClone.GetComponent<Cover>();
            pCover.UpdateArt(pFile);
            _pListCovers.Add(pCover);
        }
    }
}
