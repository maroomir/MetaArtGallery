using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private string moveAxisName = "Vertical";
    [SerializeField] private string rotateAxisName = "Horizontal";
    [SerializeField] private float moveSpeed = 5F;
    [SerializeField] private float rotateSpeed = 180F;

    private Rigidbody _pBody;
    private Animator _pAnimator;
    private float _fInputMovement;
    private float _fInputAngle;
    
    // Start is called before the first frame update
    private void Start()
    {
        _pBody = GetComponent<Rigidbody>();
        _pAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        _fInputMovement = Input.GetAxis(moveAxisName);
        _fInputAngle = Input.GetAxis(rotateAxisName);
    }

    private void FixedUpdate()
    {
        Vector3 pMoveDistance = _fInputMovement * transform.forward * moveSpeed * Time.fixedDeltaTime;
        float fRotateAngle = _fInputAngle * rotateSpeed * Time.fixedDeltaTime;
        _pBody.MovePosition(_pBody.position + pMoveDistance);
        _pBody.rotation *= Quaternion.Euler(0F, fRotateAngle, 0F);
        
        _pAnimator.SetFloat("Move", _fInputMovement);
    }
}
