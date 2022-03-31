using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Attack")
        {
            var agent = transform.parent.gameObject.GetComponent<AgentController>();
            agent.TakeHit();
        }
    }
}
