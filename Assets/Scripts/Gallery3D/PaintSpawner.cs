using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Gallery3D
{
    public class PaintSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject paint;
        [SerializeField] private int paintNum;
        [SerializeField] private float paintWidth = 3f;

        private List<Paint> _pListPaints;
        private Transform _pTransform;
        private int _iPaintCount;

        public void Init()
        {
            _pTransform = GetComponent<Transform>();
            
            _pListPaints ??= new List<Paint>();
            foreach (Paint pPaint in _pListPaints)
            {
                Destroy(pPaint.gameObject);
            }

            _pListPaints?.Clear();
        }

        public void UpdatePaints(DirectoryInfo pDirectory)
        {
            foreach (DirectoryInfo pDir in pDirectory.GetDirectories())
                UpdatePaints(pDir);
            foreach (FileInfo pFile in pDirectory.GetImages())
            {
                if (_iPaintCount >= paintNum)
                    break;
                SpawnPaint(pFile, _iPaintCount);
                _iPaintCount++;
            }
        }

        private void SpawnPaint(FileInfo pFile, int nIndex)
        {
            GameObject pClone = Instantiate(paint, _pTransform, false);
            pClone.transform.localScale = pClone.transform.localScale.EachDivide(_pTransform.localScale);
            pClone.transform.localPosition = new Vector3((nIndex - 1) * paintWidth, 0f, 0f);

            Paint pPaint = pClone.GetComponent<Paint>();
            pPaint.Init(pFile);
            _pListPaints.Add(pPaint);
        }
    }
}