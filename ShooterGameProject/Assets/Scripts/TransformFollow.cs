using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TransformFollow : MonoBehaviour
{
    [field:SerializeField] public Transform Target { get; private set; }
    [SerializeField] private bool active;
    private void FixedUpdate()
    {
        if (active)
        {
            transform.position = Target.position;
            transform.rotation = Target.rotation;
        }
    }

    public void SetTarget(Transform target)
    {
        if (target == null)
        {
            active = false;
        }
        else
        {
            active = true;
            Target = target;
        }
    }
}
