using System.Collections;
using UnityEngine;

public class BaseLocomotion
{
    public IEnumerator RotateTowards(Transform origin, Vector3 targetDirection, float rotationSpeed)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        while (Quaternion.Angle(origin.rotation, targetRotation) > 0.1f)
        {
            origin.rotation = Quaternion.Slerp(
                origin.rotation, 
                targetRotation, 
                rotationSpeed * Time.deltaTime
            );

            yield return null;
        }

        origin.rotation = targetRotation;
    }
    public void Move(Transform origin,Vector3 velocity)
    {
        origin.position += velocity * Time.deltaTime;
    }
}
