using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : AgentController
{
    /// <summary>
    /// Retourne la valeur de l'axe vertical du clavier
    /// </summary>
    /// <seealso cref="AgentController.GetInputVertical"/>
    /// <returns></returns>
    protected override float GetInputVertical()
    {
        if (Input.GetKey(KeyCode.DownArrow))
            return -1;
        return Input.GetKey(KeyCode.UpArrow) ? 1 : 0;
    }
    
    /// <summary>
    /// Retourne la valeur de l'axe vertical du clavier
    /// </summary>
    /// <seealso cref="AgentController.GetInputHorizontal"/>
    /// <returns></returns>
    protected override float GetInputHorizontal()
    {
        if (Input.GetKey(KeyCode.RightArrow))
            return 1;
        return Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;
    }
}
