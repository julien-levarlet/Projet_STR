using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOfMap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = new Vector3(0, 20, 0);
    }
}
