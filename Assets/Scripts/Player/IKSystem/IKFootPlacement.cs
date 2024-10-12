using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootPlacement : MonoBehaviour
{
    [Range(0,1)]
    public float distanceToGround;

    public LayerMask layer;
    
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (_animator)
        {
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot,1f);
            _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot,1f);

            RaycastHit hit;
            Ray ray = new Ray(_animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up,Vector3.down);

            if (Physics.Raycast(ray,out hit, distanceToGround + 1f,layer))
            {
                if (hit.transform.tag == "walkable")
                {
                    Vector3 footPosition = hit.point;
                    footPosition.y += distanceToGround;
                    _animator.SetIKPosition(AvatarIKGoal.LeftFoot,footPosition);
                }
            }
        }
    }
}
