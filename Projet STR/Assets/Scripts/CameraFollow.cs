using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    //vitesse de suivi de la cam�ra
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}
