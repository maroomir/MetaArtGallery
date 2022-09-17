using System;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Gallery3D
{
    public class PaintReceiver : RestBase
    {
        [SerializeField] private new string tag = "Painters";

        public void Awake()
        {
            Application.runInBackground = true;
            // Bring all spawners
            int nTotalCount = 0;
            List<SpawnerContainer> pListSpawners = new List<SpawnerContainer>();
            foreach (GameObject pObject in GameObject.FindGameObjectsWithTag(tag))
            {
                SpawnerContainer pContainer = new SpawnerContainer();
                pContainer.Spawner = pObject.GetComponent<PaintSpawner>();
                pContainer.PaintNum = pContainer.Spawner.PrefabNum;
                nTotalCount += pContainer.PaintNum;
                pListSpawners.Add(pContainer);
            }

            // Get paints from the server
            string strReceivedData = Get($"/images/{nTotalCount}");
            if (!RestFunctions.IsEncodedImages(strReceivedData))
                return;
            string[] pImagesEncoded = RestFunctions.SplitImages(strReceivedData);
            // Initialize the spawner
            int nStartPos = 0;
            for (int i = 0; i < pListSpawners.Count; i++)
            {
                string[] pSlices = pImagesEncoded.Slice(nStartPos, nStartPos + pListSpawners[i].PaintNum);
                pListSpawners[i].Spawner.Init();
                pListSpawners[i].Spawner.UpdatePrefabs(pSlices);
                nStartPos += pListSpawners[i].PaintNum;
            }
        }
    }
}