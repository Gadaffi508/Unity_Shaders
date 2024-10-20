using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    private Vector3 _inputDirection;
    private Rigidbody rb;

    private float horizontal, vertical;

    private Animator _animator;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        _animator.SetFloat("velocity",rb.velocity.sqrMagnitude);
    }

    void FixedUpdate()
    {
        MovePlayer();
    }
    
    void MovePlayer()
    {
        
        _inputDirection = new Vector3(horizontal, 0, vertical);
        
        if (_inputDirection != Vector3.zero)
        {
            Vector3 movement = _inputDirection * speed * Time.fixedDeltaTime;
            rb.velocity = movement;

            RotateTowardsMovement();
        }
    }
    
    void RotateTowardsMovement()
    {
        Vector3 lookDirection = new Vector3(_inputDirection.x, 0, _inputDirection.z);

        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, 0.1f));
        }
    }
}
