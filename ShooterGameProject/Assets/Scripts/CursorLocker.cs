using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLocker : MonoBehaviour
{
    public bool Locked;
    void Start()
    {
        if (Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

}
