using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public GameObject target;
    public float offsetX = 0.0F;
    public float offsetY = 0.5F;
    public float offsetZ = 1.5F;
    private Vector3 _pMovePos;

    // Start is called before the first frame update
    void Start()
    {
        _pMovePos = new Vector3(offsetX, offsetY, offsetZ);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position + _pMovePos;
    }
}
