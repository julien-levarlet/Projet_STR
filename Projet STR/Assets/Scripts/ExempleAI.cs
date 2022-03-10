using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Classe présentant un exemple de l'utilisation de <c>AgentController</c>
/// Dans cet exemple l'IA se déplace de façon complètement aléatoire
/// </summary>
public class ExempleAI : AgentController
{
    private float _horizontal;
    private float _vertical;
    
    protected override void Start()
    {
        base.Start(); // ne pas oublier l'appel à base.Start sinon le CharacterController ne sera pas chargé
        GetTarget(); // si on doit se baser sur la position du joueur, il faut d'abord le trouver
        _horizontal = 0f;
        _vertical = 0f;
    }

    /// <summary>
    /// Exemple de fonction permettant de calculer la trajectoire de l'agent
    /// </summary>
    private void Think()
    {
        Vector3 target_pos = Target.position; // position de notre cible
        _horizontal = Random.value * 2 - 1; //valeur random entre -1 et 1
        _vertical = Random.value * 2 - 1;
    }
    
    protected override float GetInputVertical()
    {
        Think(); // GetInputVertical est toujours appelée en premier
        return _vertical;
    }
    
    protected override float GetInputHorizontal()
    {
        return _horizontal;
    }
}
