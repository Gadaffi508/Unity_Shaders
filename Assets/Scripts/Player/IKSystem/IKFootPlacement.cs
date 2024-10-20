using System;
using System.Collections.Generic;
using UnityEngine;

public class IKFootPlacement : MonoBehaviour
{
    [Header("Feet Grounder")] public bool enableFeetIk = true;
    [Range(0, 2)] public float heightFromGroundRaycast = 1.14f;
    [Range(0, 2)] public float raycastDownDistance = 1.5f;

    public LayerMask environmentLayer;

    public float pelvisOffset = 0;

    [Range(0, 1)] public float pelvisUpAndDownSpeed = 0.28f;
    [Range(0, 1)] public float feetToIkPosSpeed = 0.5f;

    public string leftFootAnimVariableName = "LeftFootCurve";
    public string rightFootAnimVariableName = "RightFootCurve";

    public bool useProIkFeature = false;
    public bool showSolverDebug = true;
    
    private float lastPelvisPosY,
        lastRightFootPosY,
        lastLeftFootPosY;

    private Quaternion leftFootIkRot,
        rightFootIkRot;

    private Vector3 rightFootPos,
        leftFootPos,
        leftFootIkPos,
        rightFootIkPos;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (enableFeetIk == false) return;
        
        if(_animator == null) return;
        
        AdjustFeettarget(ref rightFootPos, HumanBodyBones.RightFoot);
        AdjustFeettarget(ref leftFootPos, HumanBodyBones.LeftFoot);
        
        FeetPositionSolver(rightFootPos, ref rightFootIkPos,ref rightFootIkRot);
        FeetPositionSolver(leftFootPos, ref leftFootIkPos,ref leftFootIkRot);
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (enableFeetIk == false) return;
        
        if(_animator == null) return;

        MovePelvisHeight();
        
        //Right foot
        _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot,1f);

        if (useProIkFeature)
        {
            _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot,_animator.GetFloat(rightFootAnimVariableName));
        }

        MoveFeetToIkPoint(AvatarIKGoal.RightFoot, rightFootIkPos, rightFootIkRot, ref lastRightFootPosY);
        
        //Left foot
        _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot,1f);

        if (useProIkFeature)
        {
            _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot,_animator.GetFloat(leftFootAnimVariableName));
        }

        MoveFeetToIkPoint(AvatarIKGoal.LeftFoot, leftFootIkPos, leftFootIkRot, ref lastLeftFootPosY);
    }

    void MoveFeetToIkPoint(AvatarIKGoal foot, Vector3 positionIkHolder, Quaternion rotationIkHolder, ref float lastFootPositionY)
    {
        Vector3 targetIkPosition = _animator.GetIKPosition(foot);

        if (positionIkHolder != Vector3.zero)
        {
            targetIkPosition = transform.InverseTransformPoint(targetIkPosition);
            positionIkHolder = transform.InverseTransformPoint(positionIkHolder);

            float yVariable = Mathf.Lerp(lastFootPositionY, positionIkHolder.y, feetToIkPosSpeed);

            targetIkPosition.y += yVariable;
            lastFootPositionY = yVariable;
            targetIkPosition = transform.TransformPoint(targetIkPosition);
            
            _animator.SetIKRotation(foot, rotationIkHolder);
        }
        
        _animator.SetIKPosition(foot,targetIkPosition);
    }

    void MovePelvisHeight()
    {
        if (rightFootIkPos == Vector3.zero || leftFootIkPos == Vector3.zero || lastPelvisPosY == 0)
        {
            lastPelvisPosY = _animator.bodyPosition.y;
            return;
        }

        float lOffsetPosiiton = leftFootIkPos.y - transform.position.y;
        float rOffsetPosition = rightFootIkPos.y - transform.position.y;

        float totalOffset = (lOffsetPosiiton < rOffsetPosition) ? lOffsetPosiiton : rOffsetPosition;

        Vector3 newPelvisPosition = _animator.bodyPosition + Vector3.up * totalOffset;

        newPelvisPosition.y = Mathf.Lerp(lastPelvisPosY, newPelvisPosition.y, pelvisUpAndDownSpeed);

        _animator.bodyPosition = newPelvisPosition;

        lastPelvisPosY = _animator.bodyPosition.y;
    }

    void FeetPositionSolver(Vector3 fromSkyPosition, ref Vector3 feetIkPositions, ref Quaternion feetIkRotations)
    {
        RaycastHit feetoutHit;
        if (showSolverDebug)
        {
            Debug.DrawLine(fromSkyPosition, fromSkyPosition + Vector3.down * (raycastDownDistance + heightFromGroundRaycast), Color.yellow);
        }

        if (Physics.Raycast(fromSkyPosition, Vector3.down, out feetoutHit, raycastDownDistance + heightFromGroundRaycast, environmentLayer))
        {
            feetIkPositions = fromSkyPosition;
            feetIkPositions.y = feetoutHit.point.y + pelvisOffset;
            feetIkRotations = Quaternion.FromToRotation(Vector3.up, feetoutHit.normal) * transform.rotation;
            
            return;
        }

        feetIkPositions = Vector3.zero;
    }

    void AdjustFeettarget(ref Vector3 feetPos, HumanBodyBones foot)
    {
        feetPos = _animator.GetBoneTransform(foot).position;

        feetPos.y = transform.position.y + heightFromGroundRaycast;
    }
}