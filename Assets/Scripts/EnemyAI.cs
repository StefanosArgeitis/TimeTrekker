using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
   
   public NavMeshAgent agent;
   public Transform player;

    public LayerMask is_ground, is_player;

    public GameObject projectile;

    /// Enemy to patrol
    public Vector3 walkPoint;

    bool walkPointSet;

    public float walkPointDistance;

    //bool canInvoke = true;

    //public float idleTime;
    //bool idled = true;

    /// Enemy to attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    /// States of the Enemy
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;


    private void Awake()
    {

        player = GameObject.Find("PlayerObj").transform;

        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        /// Checks for player in sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, is_player);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, is_player);

        /// If player is not in range then patrol
        if(!playerInSightRange && !playerInAttackRange){

            Patroling();

        }

        /// If player is not in attackrange then chase player
        if(playerInSightRange && !playerInAttackRange){

            ChasePlayer();

        }

        /// If player is in range then attack
        if(playerInSightRange && playerInAttackRange){

            AttackPlayer();

        }
    }



    private void Patroling(){

        if(!walkPointSet){

            SearchWalkPoint();

        }

        if(walkPointSet){
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        /// Will stop if walkpoint is set
        if(distanceToWalkPoint.magnitude < 1f){

            walkPointSet = false;
        }
    }

    private void Idle(){



    }

    private void SearchWalkPoint(){

        /// Walks to random point in Range
        float randomZ = Random.Range(-walkPointDistance, walkPointDistance);
        float randomX = Random.Range(-walkPointDistance, walkPointDistance);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        /// checks to see if the walkpoint is on the map
        if(Physics.Raycast(walkPoint, -transform.up, 2f, is_ground)){

            walkPointSet = true;
        }

    }

    private void ChasePlayer(){
        
        /// Will move towards the player
        agent.SetDestination(player.position);
    
    }

    private void AttackPlayer(){

        ///Stops moving when attacking
        agent.SetDestination(transform.position);
        
        transform.LookAt(player);

        if(!alreadyAttacked){

            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            ///rb.AddForce(transform.up * 8f, ForceMode.Impulse);


            alreadyAttacked = true;
            Invoke("ResetAttack", timeBetweenAttacks);
        }
    }

    private void ResetAttack(){

        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected() {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        
    }

}
