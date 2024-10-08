using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerCombat : MonoBehaviour
{
    public LayerMask enemyLayer;

    public bool isDone = false;

    private Animator _animator;

    private PlayerMovement _movement;
    private Camera _camera;
    private GameObject _currentEnemy;
    private Vector3 _inputDirection;

    private int randomIndex = 0;

    void Start()
    {
        _movement = GetComponent<PlayerMovement>();
        _camera = Camera.main;
        _animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        DetectEnemy();

        if (Input.GetMouseButton(0) && _currentEnemy != null && isDone == false)
        {
            _movement.SetRotationLock(true);
            _movement.ResetRigidbody();
            MoveTowardsTarget(_currentEnemy.transform);
        }
        else
        {
            _movement.SetRotationLock(false);
            UpdateInputDirection();
        }
    }

    public void AnimationCompleteHandler()
    {
        isDone = false;
    }
    
    void UpdateInputDirection()
    {
        if (isDone == true) return;
        
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

        float distance;
        randomIndex = Random.Range(0, 2);
        if (randomIndex == 1)
            distance = 1.6f;
        else
            distance = 2;
        
        _animator.SetTrigger("Attack");
        _animator.SetInteger("ındex",randomIndex);
    
        Vector3 lookPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.DOLookAt(lookPosition, 0.2f);

        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Vector3 targetPositionWithOffset = target.position - directionToTarget * distance;

        transform.DOMove(new Vector3(targetPositionWithOffset.x, transform.position.y, targetPositionWithOffset.z), 0.5f)
            .OnComplete(() =>
            {
                _movement.ResetRigidbody();
                Debug.Log("Move Towards Target");
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
