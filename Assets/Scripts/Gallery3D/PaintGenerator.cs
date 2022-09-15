using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using CoverFlow2D;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

namespace Gallery3D
{
    public class PaintGenerator : MonoBehaviour
    {
        [SerializeField] private new string tag = "Painters";

        private void Awake()
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

            // Bring paints as number of total count
            DirectoryInfo pDirectory = new DirectoryInfo(GlobalParameter.ResourcePath);
            FileInfo[] pFiles = Curate(pDirectory.GetImagesAll(), nTotalCount);
            // Initialize the spawner
            int nStartPos = 0;
            for (int i = 0; i < pListSpawners.Count; i++)
            {
                FileInfo[] pFilesSlice = pFiles.Slice(nStartPos, nStartPos + pListSpawners[i].PaintNum);
                pListSpawners[i].Spawner.Init();
                pListSpawners[i].Spawner.UpdatePrefabs(pFilesSlice);
                pListSpawners[i].Spawner.OnTextureDeliverEvent += OnDeliverTexture;
                nStartPos += pListSpawners[i].PaintNum;
            }
        }

        private static T[] Curate<T>(T[] pSources, int nArtNum)
        {
            if (pSources.Length < nArtNum)
                throw new Exception($"Not enough arts compared to the exhibition ({pSources.Length} < {nArtNum})");
            Random pRandom = new Random();
            IOrderedEnumerable<T> pRandomizedSource = pSources.OrderBy(item => pRandom.Next());
            T[] pResult = pRandomizedSource.ToArray();
            return pResult.Take(nArtNum).ToArray();
        }

        public void OnDeliverTexture(object pSender, TextureArgs pArgs)
        {
            GlobalParameter.SelectedTexture = pArgs.Texture;
            GlobalParameter.SelectedTextureName = pArgs.Tag;
            GlobalParameter.SelectedTexturePath = pArgs.AccessPath;
            Debug.Log($"Access the CoverFlow2D with {pArgs.Tag}");
            SceneManager.LoadScene("CoverFlow2D");
        }
    }
}