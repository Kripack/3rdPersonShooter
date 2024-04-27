using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition3D : MonoBehaviour
{
    public static MousePosition3D Instance { get; private set; }

    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask aimMask;

    private void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = _camera.ScreenPointToRay(screenCenterPoint); // or Input.mousePosition or Mouse.current.position.ReadValue();
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimMask))
        {
            transform.position = hit.point;
        }
    }

    public static Vector3 GetMouseWorldPosition() 
    { 
        if (Instance == null) { Debug.LogError("MousePosition3D object does not exist."); }
        return Instance.GetMouseWorldPosition_Instance();        
    } 

    private Vector3 GetMouseWorldPosition_Instance()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = _camera.ScreenPointToRay(screenCenterPoint); // or Input.mousePosition or Mouse.current.position.ReadValue();
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimMask))
        {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
