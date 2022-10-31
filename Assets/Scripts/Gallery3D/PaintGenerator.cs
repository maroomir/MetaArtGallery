using System.Collections.Generic;
using System.IO;
using Common;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            FileInfo[] pFiles = CurateFunctions.RandomCurate(pDirectory.GetImagesAll(), nTotalCount);
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

        public void OnDeliverTexture(object pSender, TextureArgs pArgs)
        {
            GlobalParameter.SelectedTexture = pArgs.Texture;
            GlobalParameter.SelectedTextureName = pArgs.Tag;
            GlobalParameter.SelectedTexturePath = Directory.GetParent(pArgs.AccessPath)?.ToString();
            Debug.Log($"Access the CoverFlow2D with {pArgs.Tag}");
            SceneManager.LoadScene("CoverFlow2D");
        }
    }
}