using System.Collections;
using UnityEngine;

public class BaseLocomotion
{
    private readonly Transform _origin;
    private bool _stopped;

    public BaseLocomotion(Transform transform)
    {
        _origin = transform;
    }
    public IEnumerator RotateTowards(Vector3 targetDirection, float rotationSpeed)
    {
        var targetRotation = Quaternion.LookRotation(targetDirection);

        while (Quaternion.Angle(_origin.rotation, targetRotation) > 0.1f)
        {
            _origin.rotation = Quaternion.Slerp(
                _origin.rotation, 
                targetRotation, 
                rotationSpeed * Time.fixedDeltaTime);

            yield return null;
        }

        _origin.rotation = targetRotation;
    }

    public void Rotate(Vector3 targetDirection, float rotationSpeed)
    {
        var targetRotation = Quaternion.LookRotation(targetDirection);
        
        _origin.rotation = Quaternion.Slerp(
            _origin.rotation,
            targetRotation,
            rotationSpeed + Time.deltaTime);
    }
    
    public void Move(Vector3 velocity)
    {
        if (_stopped) return;
        _origin.position += velocity * Time.fixedDeltaTime;
    }

    public void MoveTo(Vector3 position)
    {
        if (_stopped) return;
        _origin.position = position;
    }

    public void Stopped(bool status)
    {
        _stopped = status;
    }
}
