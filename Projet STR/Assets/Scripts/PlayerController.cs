using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : AgentController
{
    protected override float GetInputVertical()
    {
        return Input.GetAxis("Vertical");
    }

    protected override float GetInputHorizontal()
    {
        return Input.GetAxis("Horizontal");
    }
}
