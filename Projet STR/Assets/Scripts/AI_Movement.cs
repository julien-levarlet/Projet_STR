using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random; 

public class AI_Movement : MonoBehaviour
{

    
    
    /// <summary>
    /// /////////A* EST A PLACER DANS UPDATE A LA PLACE DE LA FONCTION CHASE()/////////////////////////////
    /// </summary>
    /* Patrol */
    public NavMeshAgent navMeshAgent;
    public bool onPatrol;
    public Transform _player;
    public float distance;
    [Range(0,100)] public float walkSPeed = 3;
    [Range(0,100)] public float runSpeed = 5;
    [Range(1, 500)] public float walkRadius;
    
    /* Champ de vision*/
    public float viewAngle = 90;
    public bool playerInSight;
    
    /*Chasing */
    public float lastAttackTime = 0;
    public float cooldown = 2;
    public bool playerCaught;



     public void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        onPatrol = true;
        navMeshAgent.speed = walkSPeed;
        navMeshAgent.SetDestination(RandomNavMeshLocation()); 
        playerCaught = false; 
        
    }

    public void Update()
    { 
        distance = Vector3.Distance(_player.transform.position, navMeshAgent.transform.position);
        if (isPlayerInsight() && !isPlayerReachable())
        {
            onPatrol = false;
            //A remplacer avec A*
            Chase();

        }
        /*else if(AttackCondition())
        {
            playerCaught = true;
            Stop();
            //Attaque
            lastAttackTime = Time.time;
        }*/
        else
        {
            onPatrol = true;
            playerCaught = false;
            Patroling();
        }
    }

    //////////////////////////////////////////*  Chase *//////////////////////////////////////// 

    /* Fonction de chasse du joueur par l'ennemi*/
    public void Chase()
    { 
        Move(runSpeed);
        playerCaught = false;
        navMeshAgent.SetDestination(_player.transform.position);
    }
    
    public  bool AttackCondition()
    {
        return (isPlayerReachable() && (Time.time - lastAttackTime >= cooldown));
    }
    

    /* Fonction qui vérifie si le joueur est dans la ligne de vue de l'ennemi */
    public bool isPlayerInsight()
    {
        Vector3 toTarget = _player.position - transform.position;
        if (Vector3.Angle(transform.forward, toTarget) <= viewAngle){
            if (Physics.Raycast(transform.position, toTarget, out RaycastHit hit, 15))
            {
                if (hit.transform.root == _player)
                {
                    playerInSight = true;
                    transform.LookAt(_player.transform);
                }
                else
                {
                    playerInSight = false;
                }
            }
        }
        else
        {
            playerInSight = false;
        }

        return playerInSight;
    }

    /* Fonction qui vérifie si le joueur est atteignable pour attaquer */
    public bool isPlayerReachable()
    {
        return (distance <= 1.5f);
    }
    
    
    //////////////////////////////////////////*  Patrouille *////////////////////////////////////////
    private void Move(float speed)
    {
        //defini la vitesse de mouvement de l'ennemi et le fait avancer sur la maap
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }
    
    
    private void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }
    
    public Vector3 RandomNavMeshLocation()
    {
        Vector3 finalposition = Vector3.zero;
        Vector3 randomPosition = Random.insideUnitSphere  * walkRadius;
        randomPosition += transform.position;
        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, walkRadius, 1))
        {
            finalposition = hit.position;
        }

        return finalposition;
    }
    
    
    void Patroling()
    {
       if (navMeshAgent != null && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
       {
           onPatrol = true;
           Move(walkSPeed);
           navMeshAgent.SetDestination(RandomNavMeshLocation());
       }
    }



}
