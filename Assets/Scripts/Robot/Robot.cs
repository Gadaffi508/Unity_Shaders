using System;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public GameObject bullet;

    public Transform firePos;
    
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject _bullet = Instantiate(bullet,firePos.position,Quaternion.identity);
            _bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 1000);
            _animator.SetTrigger("Fire");
        }
    }
}
