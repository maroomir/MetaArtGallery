using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using UnityEngine;

namespace Common
{
    public static class GlobalParameter
    {
        public static Texture SelectedTexture = null;
        public static string SelectedTextureName = string.Empty;
        public static string SelectedTexturePath = string.Empty;
        public static readonly string ScenePath = Path.Combine(Application.dataPath, "Scenes");
        public static readonly string ResourcePath = Path.Combine(Application.dataPath, "Resources");
    }

    public abstract class SpawnerBase : MonoBehaviour, ISpawner
    {
        [SerializeField] protected int imageNum;
        private int _iImageCount;

        public int ArtNum => imageNum;
    
        public abstract void Init();
        public abstract void SpawnArt(FileInfo pFile, int nIndex);

        public void UpdateArts(DirectoryInfo pDirectory)
        {
            foreach (DirectoryInfo pDir in pDirectory.GetDirectories())
                UpdateArts(pDir);
            UpdateArts(pDirectory.GetImages());
        }

        public void UpdateArts([NotNull] IEnumerable<FileInfo> pFiles)
        {
            if (pFiles == null)
                throw new ArgumentNullException(nameof(pFiles));
            foreach (FileInfo pFile in pFiles)
            {
                if (_iImageCount >= imageNum)
                    break;
                SpawnArt(pFile, _iImageCount);
                _iImageCount++;
            }
        }
    }
}