using UnityEngine;

/// <summary>
/// Mouvements de camera suivant a la premiere personne
/// </summary>
public class CameraFirstPerson : MonoBehaviour
{
    [SerializeField] private Transform target;

    void FixedUpdate()
    {
        transform.LookAt(target);
        transform.rotation = target.rotation;
    }
}
