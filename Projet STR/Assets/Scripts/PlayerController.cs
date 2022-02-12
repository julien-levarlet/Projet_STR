using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : AgentController
{
    protected override float GetDeltaPosition()
    {
        return Input.GetAxis("Vertical");
    }

    protected override float GetDeltaRotation()
    {
        return Input.GetAxis("Horizontal");
    }
}
