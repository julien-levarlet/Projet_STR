using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy2 : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform _player;
    public Text enemyState;
    public bool playerNear;
    public bool playerCaught;
    public bool onPatrol;
    public bool playerinSight;
    public float distance;
    //Vector3 playerLastPosition = Vector3.zero;
    //Vector3 playerPosition;
    public float walkSPeed = 6;
    public float runSpeed = 9;
    public float lastAttackTime = 0; //ya cb de temps la derniere attaque a eu lieu
    public float cooldown = 2; //cooldown pour l'ennemi 2sec
    public Transform[] waypoints;
    public int waypointIndex;
    
    
    /*Patrol*/
    [SerializeField] float speed = 10f;
    [SerializeField] float waitTime = 3f;
    float currentWaitTime = 0f;
    float minX, maxX, minZ, maxZ;
    Vector3 moveSpot;

        private void GetGroundSize() {
            GameObject ground = GameObject.FindWithTag("Ground");
            Renderer groundSize = ground.GetComponent<Renderer>();
            minX = (groundSize.bounds.center.x - groundSize.bounds.extents.x);
            maxX = (groundSize.bounds.center.x + groundSize.bounds.extents.x);
            minZ = (groundSize.bounds.center.z - groundSize.bounds.extents.z);
            maxZ = (groundSize.bounds.center.z + groundSize.bounds.extents.z);
        }

        Vector3 GetNewPosition()
        {
            float randomX = Random.Range(minX, maxX);
            float randomZ = Random.Range(minZ, maxZ);
            Vector3 newPosition = new Vector3(randomX, transform.position.y, randomZ);
            return newPosition;
        }

        void GetToStepping()
        {
            transform.position = Vector3.MoveTowards(transform.position, moveSpot, speed * Time.deltaTime);

            if(Vector3.Distance(transform.position, moveSpot) <= 0.2f)
            {
                if(currentWaitTime <= 0 )
                {
                    moveSpot = GetNewPosition();
                    currentWaitTime = waitTime;
                }else{
                    currentWaitTime -= Time.deltaTime;
                }
            }
        }

        void WatchYourStep()
        {
            Vector3 targetDirection = moveSpot - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 0.3f, 0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
        // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        //distance = Vector3.Distance(player.position, transform.position);
        playerNear = false;
        playerCaught = false;
        onPatrol = true;
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = walkSPeed;
        waypointIndex = 0;
        //test de destination
        Vector3 test = Vector3.zero;
        navMeshAgent.SetDestination(test);
        
        //patrol
        GetGroundSize();
        moveSpot = GetNewPosition();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(_player.transform.position, 
            navMeshAgent.transform.position);
        Chase();
        
        WatchYourStep();
        GetToStepping();
    }


    /* Fonction qui gere l'etat de l'ennemi :
             chasse le joueur, recherche le joueur ou patrouille sur l'arene */
    public void EnemyState(int state)
    {
        switch (state)
        {
            case 1:
                enemyState.text = "Chasing";
                enemyState.color = Color.red;
                break;
            case 2:
                enemyState.text = "Scanning";
                enemyState.color = Color.yellow;
                break;
            case 3:
                enemyState.text = "Patrol";
                enemyState.color = Color.blue;
                break;
            default:
                enemyState.text = "";
                break;
        }
    }
    
    /* Fonction de chasse du joueur par l'ennemi*/
    public void Chase()
    {
        playerNear = false;
        playerCaught = false;
        
        //si il repere le joueur il court dans sa direction
        if (isPlayerInsight() && !playerCaught)
        {
            Move(runSpeed);
            navMeshAgent.SetDestination(_player.transform.position);
            EnemyState(2);
            playerNear = true;
        }
        
        //si le joueur est a porte il attaque et restart le cooldown
        if (isPlayerReachable() && !playerCaught)
        {
            //Attaquer
            playerCaught = true;
            EnemyState(1);
            lastAttackTime = Time.time;
        }

        else
        {
            playerCaught = false;
            navMeshAgent.isStopped = true;
            navMeshAgent.speed = 0;
        }
    }
    
    /*Fonction pour faire bouger l'ennemi à une certaine vitesse*/
    void Move(float speed)
    {
        //defini la vitesse de mouvement de l'ennemi et le fait avancer sur la maap
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }
    

    /* Fonction qui vérifie si le joueur est à proximité de l'ennemi */
    public bool isPlayerInsight()
    {
        //si la distance est inferieur a 5m = player detecté
        if (distance <= 5 && distance > 1.5f)
        {
            playerinSight = true;
            transform.LookAt(_player.transform);
        }
        else
        {
            playerinSight = false;
        }

        return playerinSight;
    }

    /*Fonction qui verifie si le joueur est assez proche pour l'attaquer*/
    public bool isPlayerReachable()
    {
        //si le joueur est a porté pour attaquer
        if (distance <= 1.5f)
        {
            //l'ennemi arrete d'avancer s'il est sur le joueur
            navMeshAgent.isStopped = true;
            //on attaque si le cooldown est atteint 
            if (Time.time - lastAttackTime >= cooldown)
            {
                lastAttackTime = Time.time;
            }
            playerCaught = true;
        }
        else
        {
            playerCaught = false;
        }
        return playerCaught;
    }
    
    /*Faire la fonction d'attaque et de verif de la vie du joueur*/
}
