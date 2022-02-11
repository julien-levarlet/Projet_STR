using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Transform door;
    private Vector3 defaultPosition;
    private Vector3 openPosition;
    private Quaternion defaultRotation;
    private Quaternion openRotation;
    // Start is called before the first frame update
    void Start()
    {
        openPosition = new Vector3(31.5f, 1.5f, 20f);
        openRotation = Quaternion.Euler(new Vector3(0, 90, 0));
        defaultPosition = door.position;
        defaultRotation = door.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        door.position = openPosition;
        door.rotation = openRotation;
    }

    private void OnTriggerExit(Collider other)
    {
        door.position = defaultPosition;
        door.rotation = defaultRotation;
    }
}
