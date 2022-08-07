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
    public struct SpawnerContainer
    {
        public PaintSpawner Spawner;
        public int PaintNum;
    }

    public class PaintGenerator : MonoBehaviour
    {
        [SerializeField] private new string tag = "Painters";

        private int _nTotalSpawners;
        private List<SpawnerContainer> _pListSpawners;

        private void Awake()
        {
            Application.runInBackground = true;
            // Bring all spawners
            _nTotalSpawners = 0;
            _pListSpawners = new List<SpawnerContainer>();
            foreach (GameObject pObject in GameObject.FindGameObjectsWithTag(tag))
            {
                SpawnerContainer pContainer = new SpawnerContainer();
                pContainer.Spawner = pObject.GetComponent<PaintSpawner>();
                pContainer.PaintNum = pContainer.Spawner.ArtNum;
                _nTotalSpawners += pContainer.PaintNum;
                _pListSpawners.Add(pContainer);
            }

            // Initialize the spawners
            OnInitializeSpawnerEvent(this, EventArgs.Empty);
        }

        private void OnInitializeSpawnerEvent(object pSender, EventArgs pArgs)
        {
            // Bring paints as number of total count
            DirectoryInfo pDirectory = new DirectoryInfo(GlobalParameter.ResourcePath);
            FileInfo[] pFiles = Curate(pDirectory.GetImagesAll(), _nTotalSpawners);
            // Initialize the spawner
            int nStartPos = 0;
            for (int i = 0; i < _pListSpawners.Count; i++)
            {
                FileInfo[] pFilesSlice = pFiles.Slice(nStartPos, nStartPos + _pListSpawners[i].PaintNum);
                _pListSpawners[i].Spawner.Init();
                _pListSpawners[i].Spawner.UpdateArts(pFilesSlice);
                _pListSpawners[i].Spawner.OnImageSendEvent += OnSendImageEvent;
                nStartPos += _pListSpawners[i].PaintNum;
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

        public void OnSendImageEvent(object pSender, TextureArgs pArgs)
        {
            GlobalParameter.SelectedTexture = pArgs.Texture;
            GlobalParameter.SelectedTextureName = pArgs.Tag;
            GlobalParameter.SelectedTexturePath = pArgs.AccessPath;
            Debug.Log($"Access the CoverFlow2D with {pArgs.Tag}");
            SceneManager.LoadScene("CoverFlow2D");
        }
    }
}