using System.Collections;
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
        public void UpdatePrefabs([NotNull] IEnumerable<string> pEncodes);
        public void SpawnPrefab(FileInfo pFile, int nIndex);
        public void SpawnPrefab(string strImage, int nIndex);
        public GameObject CreatePrefab(int nIndex);
    }

    public interface IArt
    {
        public event DeliveryTextureHandler OnTextureDeliverEvent;
        public void OnDeliverTexture(object pSender, TextureArgs pArgs);
        
        public void UpdateArt(FileInfo pFile);
        public void UpdateArt(string strContents);
    }

    public interface IRest
    {
        public string Address { get; }

        public string Get(string strKey, int nTimeout);
        public string Post(string strKey, Dictionary<string, string> pJson, int nTimeout);
    }
}