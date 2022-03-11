using UnityEngine;

/// <summary>
/// Mouvements de camera suivant un objet
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    //vitesse de suivi de la caméra
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset = new Vector3(-5, 1, 0);
    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
        transform.rotation = target.rotation;
    }
}
