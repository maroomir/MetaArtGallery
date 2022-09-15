using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using static System.String;

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

    public abstract class RestBase : MonoBehaviour, IRest
    {
        
        [SerializeField] private string address = "127.0.0.1";
        [SerializeField] private string port = "5000";

        private bool _bFlagReceived = false;
        private DownloadHandler _pHandler = null;

        public string Address => $"{address.TrimEnd('/')}:{int.Parse(port.TrimStart(':'))}";

        public string Get(string strKey)
        {
            _bFlagReceived = false;
            strKey = $"{Address}/{strKey.TrimStart('/')}";
            StartCoroutine(GetRequest(strKey));
            while (!_bFlagReceived) Thread.Sleep(10);
            return _pHandler?.text;
        }

        private IEnumerator GetRequest(string strUri)
        {
            if (_bFlagReceived) _bFlagReceived = false;
            using UnityWebRequest pRequest = UnityWebRequest.Get(strUri);
            yield return pRequest.SendWebRequest();
            if (pRequest.responseCode >= 200 & pRequest.responseCode < 300)
                _pHandler = pRequest.downloadHandler;
            else
            {
                Debug.LogWarning($"Error Code: {pRequest.responseCode}, Message: {pRequest.error}");
                _pHandler = null;
            }

            _bFlagReceived = true;
        }

        public string Post(string strKey, Dictionary<string, string> pJson)
        {
            _bFlagReceived = false;
            strKey = $"{Address}/{strKey.TrimStart('/')}";
            WWWForm pForm = new WWWForm();
            foreach (KeyValuePair<string, string> pItem in pJson)
                pForm.AddField(pItem.Key, pItem.Value);
            StartCoroutine(PostRequest(strKey, pForm));
            while (!_bFlagReceived) Thread.Sleep(10);
            return _pHandler?.text;
        }

        private IEnumerator PostRequest(string strUri, WWWForm pForm)
        {
            if (_bFlagReceived) _bFlagReceived = false;
            using UnityWebRequest pRequest = UnityWebRequest.Post(strUri, pForm);
            yield return pRequest.SendWebRequest();
            if (pRequest.responseCode >= 200 & pRequest.responseCode < 300)
                _pHandler = pRequest.downloadHandler;
            else
            {
                Debug.LogWarning($"Error Code: {pRequest.responseCode}, Message: {pRequest.error}");
                _pHandler = null;
            }
            _bFlagReceived = true;
        }
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