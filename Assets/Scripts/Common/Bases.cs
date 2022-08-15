using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Common
{
    public abstract class ArtBase : MonoBehaviour, IArt
    {
        public event DeliveryTextureHandler OnTextureDeliverEvent;
        protected Texture _pTexture;
        protected string _strTag;
        protected string _strAccessPath;

        public void OnDeliverTexture(object pSender, TextureArgs pArgs)
        {
            OnTextureDeliverEvent?.Invoke(pSender, pArgs);
        }
        
        public abstract void UpdateArt(FileInfo pFile);
        
    }
    
    public abstract class SpawnerBase<T> : MonoBehaviour, ISpawner where T : MonoBehaviour, IArt
    {
        [SerializeField] protected int count;
        [SerializeField] protected GameObject prefab;
        [SerializeField] protected float prefabInterval;
        private int _iPrefab;
        protected List<T> _pListPrefabs;

        public int PrefabNum => count;
        public event DeliveryTextureHandler OnTextureDeliverEvent;

        public void OnDeliverTexture(object pSender, TextureArgs pArgs)
        {
            OnTextureDeliverEvent?.Invoke(pSender, pArgs);
        }
        
        public void Init()
        {
            _pListPrefabs ??= new List<T>();
            foreach (T pPrefab in _pListPrefabs)
            {
                pPrefab.OnTextureDeliverEvent -= OnTextureDeliverEvent;
                Destroy(pPrefab.gameObject);
            }

            _pListPrefabs?.Clear();
        }

        public virtual void UpdatePrefabs(DirectoryInfo pDirectory)
        {
            foreach (DirectoryInfo pDir in pDirectory.GetDirectories())
                UpdatePrefabs(pDir);
            UpdatePrefabs(pDirectory.GetImages());
        }

        public virtual void UpdatePrefabs([NotNull] IEnumerable<FileInfo> pFiles)
        {
            if (pFiles == null)
                throw new ArgumentNullException(nameof(pFiles));
            foreach (FileInfo pFile in pFiles)
            {
                if (_iPrefab >= count) break;
                SpawnPrefab(pFile, _iPrefab);
                _iPrefab++;
            }
        }

        public virtual void SpawnPrefab(FileInfo pFile, int nIndex)
        {
            GameObject pClone = CreatePrefab(nIndex);
            T pPrefab = pClone.GetComponent<T>();
            pPrefab.OnTextureDeliverEvent += OnDeliverTexture;
            pPrefab.UpdateArt(pFile);
            _pListPrefabs.Add(pPrefab);
        }

        public abstract GameObject CreatePrefab(int nIndex);
    }
}