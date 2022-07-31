using System.IO;
using CoverFlow2D;
using UnityEngine;

public class CoverGenerator : MonoBehaviour
{
    private CoverSpawner _pCoverSpawner;

    void Awake()
    {
        Application.runInBackground = true;
        string strResourcePath = Path.Combine(Application.dataPath, "Resources");
        
        _pCoverSpawner = GetComponent<CoverSpawner>();
        _pCoverSpawner.Init();
        _pCoverSpawner.UpdateCovers(new DirectoryInfo(strResourcePath));
    }
}
