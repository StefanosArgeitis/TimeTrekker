using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
   
   public NavMeshAgent agent;
   public Transform player;
   //public NavMeshPath navPath;

    public LayerMask is_ground, is_player;

    public GameObject projectile;

    /// Enemy to patrol
    public Vector3 walkPoint;

    bool walkPointSet;
    public float idleTime;

    public float walkPointDistance;
    public Transform Attackpoint;

    //bool canInvoke = true;

    //public float idleTime;
    bool idled = true;

    /// Enemy to attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    private Animator animate;

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

            animate.SetBool("ChaseR", false);
            animate.SetBool("AttackR", false);

        }

        /// If player is not in attackrange then chase player
        if(playerInSightRange && !playerInAttackRange){

            //animate.SetBool("ChaseR", true);
            //animate.SetBool("AttackR", false);

            ChasePlayer();

        }

        /// If player is in range then attack
        if(playerInSightRange && playerInAttackRange){

            AttackPlayer();
            animate.SetBool("AttackR", true);

        }
    }



    private void Patroling(){

        if (!idled){
           return;
        }

        if(!walkPointSet){

            SearchWalkPoint();

        }

        if(walkPointSet){

            NavMeshPath navPath = new NavMeshPath();

            if(agent.CalculatePath(walkPoint, navPath) &&  navPath.status == NavMeshPathStatus.PathComplete){

                agent.SetDestination(walkPoint);
                //Debug.Log("yes");

            }else {
                walkPointSet = false;
                //Debug.Log("no");
            }
            
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        /// Will stop if walkpoint is set
        if(distanceToWalkPoint.magnitude < 1f){

            walkPointSet = false;
            Idle();
        }
    }

    private void Idle(){

        idled = false;

        //agent.SetDestination(transform.position);

        Invoke("ResetIdle", idleTime);

    }

    private void ResetIdle(){
        idled = true;
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
        animate.SetBool("ChaseR", true);
        animate.SetBool("AttackR", false);
        /// Will move towards the player
        agent.SetDestination(player.position);
    
    }

    private void AttackPlayer(){
        
        animate.SetBool("AttackR", true);

        ///Stops moving when attacking
        agent.SetDestination(transform.position);
        
        transform.LookAt(player);

        if(!alreadyAttacked){

            Rigidbody rb = Instantiate(projectile, Attackpoint.position, Quaternion.identity).GetComponent<Rigidbody>();

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
