using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Common
{
    public interface ISpawner
    {
        public event DeliveryTextureHandler OnTextureDeliverEvent;
        public void OnDeliverTexture(object pSender, TextureArgs pArgs);
        
        public void Init();
        public void UpdatePrefabs(DirectoryInfo pDirectory);
        public void UpdatePrefabs([NotNull] IEnumerable<FileInfo> pFiles);
        public void SpawnPrefab(FileInfo pFile, int nIndex);
        public GameObject CreatePrefab(int nIndex);
    }

    public interface IArt
    {
        public event DeliveryTextureHandler OnTextureDeliverEvent;
        public void OnDeliverTexture(object pSender, TextureArgs pArgs);
        
        public void UpdateArt(FileInfo pFile);
    }
}