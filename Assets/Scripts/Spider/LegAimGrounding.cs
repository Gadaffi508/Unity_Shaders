using System;
using UnityEngine;

public class LegAimGrounding : MonoBehaviour
{
    public LayerMask groundLayer;
    
    private GameObject _raycastOrigin;

    private void Start()
    {
        _raycastOrigin = transform.parent.gameObject;
    }

    private void Update()
    {
        if (Physics.Raycast(_raycastOrigin.transform.position, -transform.up, out RaycastHit hit, Mathf.Infinity,groundLayer))
        {
            transform.position = hit.point;
        }
    }
}