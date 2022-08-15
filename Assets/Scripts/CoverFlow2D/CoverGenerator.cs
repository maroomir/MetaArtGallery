using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Common;
using UnityEngine;

namespace CoverFlow2D
{
    public class CoverGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject coverFlow;
        private static readonly int _nMaxNum = 20;
        private CoverSpawner _pCoverSpawner;
        private EventHandler _pMoveLeftHandler;
        private EventHandler _pMoveRightHandler;

        private void Awake()
        {
            Application.runInBackground = true;
            string strSampleImagePath = Path.Combine(GlobalParameter.ResourcePath, "YonseiEmblem.png");
            FileInfo pSourceFile = (GlobalParameter.SelectedTexturePath != string.Empty)
                ? new FileInfo(GlobalParameter.SelectedTexturePath)
                : new FileInfo(strSampleImagePath);
            FileInfo[] pFiles = Run(pSourceFile, _nMaxNum);
            _pCoverSpawner = coverFlow.GetComponent<CoverSpawner>();
            _pCoverSpawner.Init();
            _pCoverSpawner.UpdatePrefabs(pFiles);
            _pMoveLeftHandler += _pCoverSpawner.OnMoveLeft;
            _pMoveRightHandler += _pCoverSpawner.OnMoveRight;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                _pMoveLeftHandler.Invoke(this, EventArgs.Empty);
            if (Input.GetKeyDown(KeyCode.RightArrow))
                _pMoveRightHandler.Invoke(this, EventArgs.Empty);
        }

        private static T[] Run<T>(T pSource, int nArtNum)
        {
            T[] pResult = new T[nArtNum];
            for (int i = 0; i < nArtNum; i++) pResult[i] = pSource;
            return pResult;
        }
    }
}
