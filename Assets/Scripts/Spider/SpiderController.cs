using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    public float speed = 6;

    private Rigidbody _rb;

    private Vector3 _velocity;

    private float _turnSmoothVelocity;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        _rb.velocity = new(y * speed * Time.deltaTime, 0, -x * speed * Time.deltaTime);
    }
}