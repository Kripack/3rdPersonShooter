using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Serialization;
using UnityEngine.Windows;

public class ViewController : MonoBehaviour
{
    public InputReader playerInput;

    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [FormerlySerializedAs("FPSVirtualCamera")] [SerializeField] private CinemachineVirtualCamera fpsVirtualCamera;

    [Header("Follow Target")]
    [SerializeField] private Transform cameraFollowTarget;

    [Header("Sensitivity")]
    [Range(0, 30)][SerializeField] private float sensitivity;
    [Range(0, 30)][SerializeField] private float aimSensitivity;

    [Header("Vertical min/max angle")]
    [Range(-360,360)][SerializeField] private float minXAngle = -30f;
    [Range(-360,360)][SerializeField] private float maxXAngle = 70f;

    private float _normalSensitivity;
    private float _xRotation;
    private float _yRotation;

    private Camera _mainCamera;

    private void Start()
    {
        _normalSensitivity = sensitivity;
        Cursor.lockState = CursorLockMode.Locked;
        
        _mainCamera = Camera.main;

        playerInput.Aim += AimCameraSwitch;
        playerInput.ViewChange += ChangeView;
    }
    public void CameraControl()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        _xRotation += -playerInput.look.y * Time.fixedDeltaTime * sensitivity;
        _yRotation += playerInput.look.x * Time.fixedDeltaTime * sensitivity;

        _xRotation = Mathf.Clamp(_xRotation, minXAngle, maxXAngle);

        Quaternion targetRotation = Quaternion.Euler(_xRotation, _yRotation, cameraFollowTarget.rotation.eulerAngles.z);
        cameraFollowTarget.rotation = targetRotation;
    }
    private void ChangeView()
    {
        SetSensitivity(_normalSensitivity);
        fpsVirtualCamera.enabled = !fpsVirtualCamera.enabled;
    }
    public void SetSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
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
            SetSensitivity(_normalSensitivity);
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
