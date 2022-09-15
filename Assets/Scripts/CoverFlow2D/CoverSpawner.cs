using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace CoverFlow2D
{
    public class CoverSpawner : SpawnerBase<Cover>
    {
        [SerializeField] private float centralGapRatio = 1.5f;
        [SerializeField] private float centralDepth = 10f;
        [SerializeField] private float sideGapRatio = 0.5f;
        [SerializeField] private float sideAngle = 30f;
        [SerializeField] private float sideDepth = 30f;
        [SerializeField] private int frameInterval = 10;
        private readonly int _nFPS = 120;
        private float _fCurrentPos;

        public override void UpdatePrefabs([NotNull] IEnumerable<FileInfo> pFiles)
        {
            base.UpdatePrefabs(pFiles);
            _fCurrentPos = count / 2f;
            StartCoroutine(UpdateLayout());
        }

        public override GameObject CreatePrefab(int nIndex)
        {
            GameObject pClone = Instantiate(prefab, transform, false);
            pClone.transform.localScale = pClone.transform.localScale.EachDivide(transform.localScale);
            pClone.transform.localPosition = new Vector3(nIndex * prefabInterval, 0f, 0f);
            return pClone;
        }

        public void OnMoveLeft(object sender, EventArgs e)
        {
            StartCoroutine(MoveLeft());
        }

        public void OnMoveRight(object sender, EventArgs e)
        {
            StartCoroutine(MoveRight());
        }

        private IEnumerator UpdateLayout(float fOffset = 0f)
        {
            _fCurrentPos += fOffset;
            for (int i = 0; i < PrefabNum; i++)
            {
                float fIndexOffset = _fCurrentPos - i;
                _pListPrefabs[i].transform.localEulerAngles = EulerAngle(fIndexOffset, i);
                _pListPrefabs[i].transform.localPosition = Position(fIndexOffset, i);
            }

            yield return new WaitForSeconds(1f/_nFPS);
        }

        private IEnumerator MoveLeft()
        {
            if (_fCurrentPos < 0f)
                yield break;
            for (int i = 0; i < frameInterval; i++)
                yield return StartCoroutine(UpdateLayout(- 1f / frameInterval));
        }

        private IEnumerator MoveRight()
        {
            if (_fCurrentPos > count - 1)
                yield break;
            for (int i = 0; i < frameInterval; i++)
                yield return StartCoroutine(UpdateLayout(1f / frameInterval));
        }

        private Vector3 EulerAngle(float fOffset, int nIndex) => new Vector3(0, RotationAngle(fOffset), 0);

        private Vector3 Position(float fOffset, int nIndex) => new Vector3(TranslationX(fOffset), 0, TranslationZ(fOffset));

        private float RotationAngle(float fOffset) => fOffset is >= 1 or <= -1
            ? MathFunctions.Sign(fOffset) * sideAngle
            : Math.Abs(fOffset) * MathFunctions.Sign(fOffset) * sideAngle;

        private float TranslationX(float fOffset) => fOffset is >= 1 or <= -1
            ? fOffset * sideGapRatio * prefabInterval + MathFunctions.Sign(fOffset) * centralGapRatio * prefabInterval
            : MathFunctions.Sign(fOffset) * centralGapRatio * prefabInterval * Math.Abs(fOffset);

        private float TranslationZ(float fOffset) => fOffset is >= 1 or <= -1
            ? sideDepth
            : centralGapRatio * prefabInterval - Math.Abs(fOffset) * (centralDepth - sideDepth);
    }
}
