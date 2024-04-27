using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Windows;

public class ViewController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private Transform cameraFollowTarget;
    [Range(0, 30)][SerializeField] private float sensitivity;
    private float normalSensitivity;
    [Range(0, 30)][SerializeField] private float aimSensitivity;
    [Header("Vertical min/max angle")]
    [Range(-360,360)][SerializeField] private float minXAngle = -30f;
    [Range(-360,360)][SerializeField] private float maxXAngle = 70f;

    public InputReader playerInput;

    private float xRotation;
    private float yRotation;
    private bool lockCameraPosition;

    private void Start()
    {
        normalSensitivity = sensitivity;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void CameraControl()
    {
        CameraRotation();
        AimCameraSwitch();
    }
    private void AimCameraSwitch()
    {
        if (playerInput?.aim == true)
        {
            aimVirtualCamera.enabled = true;
            SetSensitivity(aimSensitivity);
        }
        else
        { 
            aimVirtualCamera.enabled = false; 
            SetSensitivity(normalSensitivity);
        }
    }

    private void CameraRotation()
    {
        xRotation += -playerInput.look.y * Time.fixedDeltaTime * sensitivity;
        yRotation += playerInput.look.x * Time.fixedDeltaTime * sensitivity;

        xRotation = Mathf.Clamp(xRotation, minXAngle, maxXAngle);

        Quaternion targetRotation = Quaternion.Euler(xRotation, yRotation, cameraFollowTarget.rotation.eulerAngles.z);
        cameraFollowTarget.rotation = targetRotation;
    }

    public void SetSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
