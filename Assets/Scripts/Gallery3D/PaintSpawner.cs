using Common;
using UnityEngine;

namespace Gallery3D
{
    public class PaintSpawner : SpawnerBase<Paint>
    {
        public override GameObject CreatePrefab(int nIndex)
        {
            GameObject pClone = Instantiate(prefab, transform, false);
            pClone.transform.localScale = pClone.transform.localScale.EachDivide(transform.localScale);
            pClone.transform.localPosition =
                (count > 1) ? new Vector3((nIndex - 1) * prefabInterval, 0f, 0f) : Vector3.zero;
            return pClone;
        }
    }
}