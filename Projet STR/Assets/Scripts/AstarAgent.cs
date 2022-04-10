using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Pathfinding;


/// <summary>
/// Classe permettant le déplacement de l'ennemi suivant l'algorithme A*.
/// Réalisé en s'inpirant de https://arongranberg.com/astar/documentation/dev_4_1_19_ae95c5a3/old/custom_movement_script.php
/// </summary>
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(NavMeshAgent))]
public class AstarAgent : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent navMeshAgent;
    private Seeker _seeker;
    /* Patrol */
    private bool _onPatrol;
    private bool _playerCaught;
    private float _distance;
    [Range(0,100)] public float walkSPeed = 3;
    [Range(0,100)] public float runSpeed = 5;
    [Range(1, 500)] public float walkRadius;
    
    /* Champ de vision*/
    public float viewAngle = 90;
    public bool playerInSight;
    
    /*Chasing */
    public float lastAttackTime;
    public float cooldown = 2;
    
    /* A* */
    public Path path;
    public float speed = 2;
    public float nextWaypointDistance = 3;
    private int currentWaypoint = 0;
    public bool reachedEndOfPath;

    public void Start()
    {
        _seeker = GetComponent<Seeker>();
        _onPatrol = true;
        _playerCaught = false;
    }

    public void Update()
    {
        if (IsPlayerInsight() && !IsPlayerReachable())
        {
            _onPatrol = false;
            //A remplacer avec A*
            _seeker.StartPath(transform.position, target.position);
            if (path == null) Patroling();
            else
            {
                float distanceToWaypoint;
                currentWaypoint = 0;
                while (true) {
                    // If you want maximum performance you can check the squared distance instead to get rid of a
                    // square root calculation. But that is outside the scope of this tutorial.
                    distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
                    if (distanceToWaypoint < nextWaypointDistance) {
                        // Check if there is another waypoint or if we have reached the end of the path
                        if (currentWaypoint + 1 < path.vectorPath.Count) {
                            currentWaypoint++;
                        } else {
                            // Set a status variable to indicate that the agent has reached the end of the path.
                            // You can use this to trigger some special code if your game requires that.
                            reachedEndOfPath = true;
                            break;
                        }
                    } else {
                        break;
                    }
                }

                navMeshAgent.SetDestination(path.vectorPath[currentWaypoint]);
            }

        }
        else if(AttackCondition())
        {
            _playerCaught = true;
            Stop();
            //Attaque
            lastAttackTime = Time.time;
        }
        else
        {
            _onPatrol = true;
            _playerCaught = false;
            Patroling();
        }
    }

    private void OnPathComplete()
    {
        
    }

    //////////////////////////////////////////*  Chase *//////////////////////////////////////// 
    #region chase
    /* Fonction de chasse du joueur par l'ennemi*/
    public void Chase()
    { 
        Move(runSpeed);
        _playerCaught = false;
        navMeshAgent.SetDestination(target.transform.position);
    }
    
    public bool AttackCondition()
    {
        return (IsPlayerReachable() && (Time.time - lastAttackTime >= cooldown));
    }
    

    /* Fonction qui vérifie si le joueur est dans la ligne de vue de l'ennemi */
    public bool IsPlayerInsight()
    {
        Vector3 toTarget = target.position - transform.position;
        if (Vector3.Angle(transform.forward, toTarget) <= viewAngle){
            if (Physics.Raycast(transform.position, toTarget, out RaycastHit hit, 15))
            {
                if (hit.transform.root == target)
                {
                    playerInSight = true;
                    transform.LookAt(target.transform);
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
    public bool IsPlayerReachable()
    {
        return (_distance <= 1.5f);
    }
    #endregion
    
    
    //////////////////////////////////////////*  Patrouille *////////////////////////////////////////

    #region Patrouille

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
            _onPatrol = true;
            Move(walkSPeed);
            navMeshAgent.SetDestination(RandomNavMeshLocation());
        }
    }

    #endregion
    
}
