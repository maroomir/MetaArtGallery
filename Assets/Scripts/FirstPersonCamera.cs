using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public float turnSpeed = 4F;
    public float moveSpeed = 1F;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Input the Mouse control
        float fMouseX = Input.GetAxis("Mouse X") * turnSpeed;
        float fMouseY = -Input.GetAxis("Mouse Y") * turnSpeed;
        Vector3 pCurrAngle = transform.eulerAngles;
        float fRotateY = pCurrAngle.y + fMouseX;
        float fRotateX = pCurrAngle.x + fMouseY;
        transform.eulerAngles = new Vector3(fRotateX, fRotateY, 0);
        // Input the Keyboard control
        Vector3 pMoveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.Translate(pMoveVector * moveSpeed * Time.deltaTime);
    }
}
