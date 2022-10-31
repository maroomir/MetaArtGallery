using System;
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
            
            // Bring all images
            DirectoryInfo pDirectory = new DirectoryInfo(GlobalParameter.SelectedTexturePath);
            FileInfo[] pFiles = CurateFunctions.RandomCurate(pDirectory.GetImagesAll(), _nMaxNum);
            // Initialize the spawner
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
    }
}
