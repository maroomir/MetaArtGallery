using System.Diagnostics.CodeAnalysis;
using System.IO;
using Common;
using UnityEngine;

namespace CoverFlow2D
{
    public class CoverGenerator : MonoBehaviour
    {
        private CoverSpawner _pCoverSpawner;
        private static readonly int _nMaxNum = 100;

        private void Awake()
        {
            Application.runInBackground = true;
            FileInfo pSourceFile = new FileInfo(GlobalParameter.SelectedTexturePath);
            FileInfo[] pFiles = Run(pSourceFile, _nMaxNum);
            _pCoverSpawner = GetComponent<CoverSpawner>();
            _pCoverSpawner.Init();
            _pCoverSpawner.UpdateArts(pFiles);
        }

        private static T[] Run<T>(T pSource, int nArtNum)
        {
            T[] pResult = new T[nArtNum];
            for (int i = 0; i < nArtNum; i++)
                pResult[i] = pSource;
            return pResult;
        }
    }
}
