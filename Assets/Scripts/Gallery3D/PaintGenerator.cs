using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoverFlow2D;
using UnityEngine;

namespace Gallery3D
{
    public struct SpawnerContainer
    {
        public PaintSpawner Spawner;
        public int PaintNum;
    }

    public class PaintGenerator : MonoBehaviour
    {
        [SerializeField] private new string tag = "Painters";

        private void Awake()
        {
            Application.runInBackground = true;
            string strResourcePath = Path.Combine(Application.dataPath, "Resources", "Paintings");
            // Bring all spawners
            int nTotalCount = 0;
            List<SpawnerContainer> pListSpawners = new List<SpawnerContainer>();
            foreach (GameObject pObject in GameObject.FindGameObjectsWithTag(tag))
            {
                SpawnerContainer pContainer = new SpawnerContainer();
                pContainer.Spawner = pObject.GetComponent<PaintSpawner>();
                pContainer.PaintNum = pContainer.Spawner.ArtNum;
                nTotalCount += pContainer.PaintNum;
                pListSpawners.Add(pContainer);
            }
            // Bring paints as number of total count
            DirectoryInfo pDirectory = new DirectoryInfo(strResourcePath);
            FileInfo[] pFiles = pDirectory.GetImagesAll();
            if (pFiles.Length < nTotalCount)
                throw new Exception($"Not enough arts compared to the exhibition ({pFiles.Length} < {nTotalCount})");
            pFiles = pFiles.Take(nTotalCount).ToArray();
            // Initialize the spawner
            int nStartPos = 0;
            for (int i = 0; i < pListSpawners.Count; i++)
            {
                FileInfo[] pFilesSlice = pFiles.Slice(nStartPos, nStartPos + pListSpawners[i].PaintNum);
                pListSpawners[i].Spawner.Init();
                pListSpawners[i].Spawner.UpdateArts(pFilesSlice);
                nStartPos += pListSpawners[i].PaintNum;
            }
        }
    }
}