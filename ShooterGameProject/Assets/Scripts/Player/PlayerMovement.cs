using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDistance;
    [SerializeField] private Transform cameraObject;
    [SerializeField] private Transform cameraFollowTarget;

    private Rigidbody rb;
    
    [SerializeField] private float jumpingForce;
    [SerializeField] private float leapingForce;
    private float instantGravity = 2f;
    private float gravityForce = 9.87f;
    private float inAirTimer = 0.1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector2 moveInput, float speed, float speedMultiplier = 1f)
    {
        Vector3 forwardVel = cameraObject.forward * moveInput.y;
        Vector3 horizontalVel = cameraObject.right * moveInput.x;
        Vector3 moveDirection = (forwardVel + horizontalVel).normalized;
        moveDirection.y = 0f;

        rb.MovePosition(transform.position + moveDirection * speed * speedMultiplier * Time.fixedDeltaTime);
    }
    public void Rotate(float rotationSpeed)
    {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = cameraObject.forward;
        targetDirection.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        transform.rotation = playerRotation;
    }
    public void RotateToMoveDir(Vector2 moveInput)
    {
        Vector3 rotateDirection = cameraObject.forward * moveInput.y;
        rotateDirection += cameraObject.right * moveInput.x;
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
        rb.AddForce(instantGravity * Vector3.down);
        if (!IsGrounded())
        {
            inAirTimer += Time.deltaTime;
            rb.AddForce(gravityForce * inAirTimer * Vector3.down);
        }
        else inAirTimer = 0;
    }
    public void Jump()
    {
        rb.AddForce(Vector3.up * jumpingForce, ForceMode.Impulse);
        //rb.AddForce(transform.forward * leapingForce, ForceMode.VelocityChange);
    }
    //public void Rotate()
    //{
    //    Vector3 worldAimTarget = MousePosition3D.GetMouseWorldPosition();
    //    worldAimTarget.y = transform.position.y;
    //    Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
    //
    //    transform.rotation = Quaternion.LookRotation(aimDirection);
    //}
}
