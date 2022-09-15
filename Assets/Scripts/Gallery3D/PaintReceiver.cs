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
            string strReceivedData = Get($"/{nTotalCount}");
            if (!RestFunctions.IsEncodedImages(strReceivedData)) return;
            // Initialize the spawner
        }
        
        
    }
}