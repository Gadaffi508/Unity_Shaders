using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float speed;

    internal float x, y;

    private Vector3 _inputDirection;
    private Rigidbody rb;

    private bool shouldRotateTowardsMovement = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        MovePlayer();
    }

    public void SetInputDirection(float horizontal, float vertical)
    {
        _inputDirection = new Vector3(horizontal, 0, vertical).normalized;

        x = horizontal;
        y = vertical;
    }

    public void SetRotationLock(bool locked)
    {
        shouldRotateTowardsMovement = !locked;
    }

    void MovePlayer()
    {
        if (_inputDirection != Vector3.zero)
        {
            Vector3 movement = _inputDirection * speed * Time.fixedDeltaTime;
            rb.velocity = movement;

            if (shouldRotateTowardsMovement)
            {
                RotateTowardsMovement();
            }
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