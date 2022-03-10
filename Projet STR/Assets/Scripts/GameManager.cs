using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe se chargeant de vérifier le bon déroulement du jeu :
/// - gestion de la sortie de la map
/// - début et fin de partie
/// - mise en place de différentes règles
/// </summary>
[RequireComponent(typeof(Collider))]
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Quand on sort de la zone de jeu, on perd un point de vie et on retourne à la position de départ
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        AgentController agent = other.gameObject.GetComponent<AgentController>();
        agent.TakeHit();
        agent.ResetPos();
    }
}