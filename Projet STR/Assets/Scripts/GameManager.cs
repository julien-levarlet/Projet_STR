using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe se chargeant de v�rifier le bon d�roulement du jeu :
/// - gestion de la sortie de la map
/// - d�but et fin de partie
/// - mise en place de diff�rentes r�gles
/// </summary>
[RequireComponent(typeof(Collider))]
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Quand on sort de la zone de jeu, on perd un point de vie et on retourne � la position de d�part
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        AgentController agent = other.gameObject.GetComponent<AgentController>();
        agent.TakeHit();
        agent.ResetPos();
    }
}