using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using JetBrains.Annotations;
using UnityEngine;

namespace Gallery3D
{
    public class PaintSpawner : SpawnerBase
    {
        public event SendImageCallback OnImageSendEvent;
        
        [SerializeField] private GameObject paint;
        [SerializeField] private float paintWidth = 3f;
        private List<Paint> _pListPaints;
        private Transform _pTransform;

        public override void Init()
        {
            _pTransform = GetComponent<Transform>();
            
            _pListPaints ??= new List<Paint>();
            foreach (Paint pPaint in _pListPaints)
            {
                pPaint.OnImageSendEvent -= OnSendImageEvent;
                Destroy(pPaint.gameObject);
            }

            _pListPaints?.Clear();
        }

        public override void SpawnArt(FileInfo pFile, int nIndex)
        {
            GameObject pClone = Instantiate(paint, _pTransform, false);
            pClone.transform.localScale = pClone.transform.localScale.EachDivide(_pTransform.localScale);
            pClone.transform.localPosition =
                (imageNum > 1) ? new Vector3((nIndex - 1) * paintWidth, 0f, 0f) : Vector3.zero;

            Paint pPaint = pClone.GetComponent<Paint>();
            pPaint.OnImageSendEvent += OnSendImageEvent;
            pPaint.UpdateArt(pFile);
            _pListPaints.Add(pPaint);
        }

        public void OnSendImageEvent(object pSender, TextureArgs pArgs)
        {
            OnImageSendEvent?.Invoke(pSender, pArgs);
        }
    }
}