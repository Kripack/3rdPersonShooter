using UnityEngine;

public class PlayerMovement : MonoBehaviour
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

    public void Move(Vector2 moveInput, float speed, float speedMultiplier = 1f)
    {
        Vector3 forwardVel = transform.forward * moveInput.y;
        Vector3 horizontalVel = transform.right * moveInput.x;

        var moveDirection = (forwardVel + horizontalVel).normalized;
        moveDirection.y = 0f;

        transform.position += moveDirection * (speed * speedMultiplier * Time.deltaTime);
    }
    public void Rotate(float rotationSpeed)
    {
        Vector3 targetDirection = cameraObject.forward;
        targetDirection.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        
        transform.rotation = playerRotation;
    }
    public void RotateToMoveDir(Vector2 moveInput)
    {
        Vector3 rotateDirection = transform.forward * moveInput.y;
        rotateDirection += transform.right * moveInput.x;
        rotateDirection.y = 0f;
        rotateDirection.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(rotateDirection);
        transform.rotation = targetRotation;
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
    public void Jump()
    {
        _rb.AddForce(Vector3.up * jumpingForce, ForceMode.Impulse);
    }

}
