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
        return Input.GetAxis("Vertical");
    }
    
    /// <summary>
    /// Retourne la valeur de l'axe vertical du clavier
    /// </summary>
    /// <seealso cref="AgentController.GetInputHorizontal"/>
    /// <returns></returns>
    protected override float GetInputHorizontal()
    {
        return Input.GetAxis("Horizontal");
    }
}
