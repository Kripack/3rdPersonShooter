using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDistance;
    [SerializeField] private Transform cameraObject;
    [SerializeField] private Transform cameraFollowTarget;

    private Rigidbody _rb;
    
    [SerializeField] private float jumpingForce;
    [SerializeField] private float leapingForce;
    [SerializeField] private float instantGravity = 2f;
    [SerializeField] private float gravityForce = 9.87f;
    [SerializeField] private float inAirTimer = 0.1f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public bool IsGrounded()
    {
        return Physics.CheckSphere(transform.position, groundDistance, groundMask);
    }
    
    public void ApplyGravity()
    {
        _rb.AddForce(instantGravity * Vector3.down);
        if (!IsGrounded())
        {
            inAirTimer += Time.deltaTime;
            _rb.AddForce(gravityForce * inAirTimer * Vector3.down);
        }
        else inAirTimer = 0;
    }
    
    public void Move(Vector2 moveInput, float speed, float speedMultiplier = 1f)
    {
        var forwardVel = transform.forward * moveInput.y;
        var horizontalVel = transform.right * moveInput.x;

        var moveDirection = (forwardVel + horizontalVel).normalized;
        moveDirection.y = 0f;

        _rb.MovePosition(_rb.position + moveDirection * (speed * speedMultiplier * Time.deltaTime));
        //transform.position += moveDirection * (speed * speedMultiplier * Time.deltaTime);
    }
    
    public void Rotate(float rotationSpeed)
    {
        var targetDirection = cameraObject.forward;
        targetDirection.y = 0f;
        var targetRotation = Quaternion.LookRotation(targetDirection);
        var playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        
        transform.rotation = playerRotation;
    }
    
    public void RotateToMoveDir(Vector2 moveInput)
    {
        var rotateDirection = transform.forward * moveInput.y;
        rotateDirection += transform.right * moveInput.x;
        rotateDirection.y = 0f;
        rotateDirection.Normalize();

        var targetRotation = Quaternion.LookRotation(rotateDirection);
        transform.rotation = targetRotation;
    }
    
    public void Jump()
    {
        _rb.AddForce(Vector3.up * jumpingForce, ForceMode.Impulse);
    }

}
