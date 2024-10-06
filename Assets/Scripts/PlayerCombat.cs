using System;
using DG.Tweening;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public LayerMask enemyLayer;
    public float stopDistance = 1f;

    public bool isDone = false;

    private PlayerMovement _movement;
    private Camera _camera;
    private GameObject _currentEnemy;
    private Vector3 _inputDirection;

    void Start()
    {
        _movement = GetComponent<PlayerMovement>();
        _camera = Camera.main;
    }

    void Update()
    {
        UpdateInputDirection();
        DetectEnemy();

        if (Input.GetMouseButton(0) && _currentEnemy != null && isDone == false)
        {
            _movement.SetRotationLock(true);
            MoveTowardsTarget(_currentEnemy.transform);
        }
        else
        {
            _movement.SetRotationLock(false);
        }
    }
    
    void UpdateInputDirection()
    {
        var forward = _camera.transform.forward;
        var right = _camera.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        _inputDirection = forward * verticalInput + right * horizontalInput;
        _inputDirection.Normalize();

        _movement.SetInputDirection(horizontalInput, verticalInput);
    }

    void DetectEnemy()
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, 3f, _inputDirection, out hitInfo, 10, enemyLayer))
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                _currentEnemy = hitInfo.collider.gameObject;
            }
        }
    }
    
    void MoveTowardsTarget(Transform target)
    {
        isDone = true;
        Vector3 lookPosition = new Vector3(target.position.x, transform.position.y, target.position.z);

        transform.DOLookAt(lookPosition, 0.2f);

        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Vector3 targetPositionWithOffset = target.position - directionToTarget * stopDistance;

        transform.DOMove(new Vector3(targetPositionWithOffset.x, transform.position.y, targetPositionWithOffset.z), 0.5f).OnComplete(
            () =>
            {
                isDone = false;
            });
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, _inputDirection);
        Gizmos.DrawWireSphere(transform.position, 1f);

        if (_currentEnemy != null)
        {
            Gizmos.DrawSphere(_currentEnemy.transform.position, 1f);
        }
    }
}
