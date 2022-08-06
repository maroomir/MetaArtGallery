using System.IO;
using UnityEngine;

namespace CoverFlow2D
{
    public class CoverGenerator : MonoBehaviour
    {
        private CoverSpawner _pCoverSpawner;

        void Awake()
        {
            Application.runInBackground = true;
            string strResourcePath = Path.Combine(Application.dataPath, "Resources");
        
            _pCoverSpawner = GetComponent<CoverSpawner>();
            _pCoverSpawner.Init();
            _pCoverSpawner.UpdateArts(new DirectoryInfo(strResourcePath));
        }
    }
}
