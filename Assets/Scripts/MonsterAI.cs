using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{

    [SerializeField] GameObject terrain;

    GameObject target;
    GameObject flower;

    MeshCollider col;

    public float viewDistance; 
    public Light spotLight;

    [HideInInspector]
    public NavMeshAgent agent;

    // For chasing
    [SerializeField] float chaseRange = 50f;
    float transfromRange = 12f;
    float distanceToTarget = Mathf.Infinity;
    float distanceToFlower = Mathf.Infinity;
    float minX, maxX, minZ, maxZ;
    float viewAngle;

    //Need Behaviors for Attack and Suspiscion
    [SerializeField] float suspiscionTime = 0f;
    float timeSinceLastSawPlayer = Mathf.Infinity;

    // enemy starting position
    Vector3 enemyPos;
    Vector3 roamPos;

    Animator roboAnim;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("Player");
        flower = GameObject.FindWithTag("Interactable");
        viewAngle = spotLight.spotAngle;
        enemyPos = this.transform.position;
        roamPos = GetRoamingPos();

        roboAnim = GetComponentInChildren<Animator>();

        col = terrain.GetComponent<MeshCollider>();

        // cannot call col.bounds in update()
        minX = col.bounds.min.x;
        maxX = col.bounds.max.x;
        minZ = col.bounds.min.z;
        maxZ = col.bounds.max.z;
    }


    // Update is called once per frame
    void Update()
    {
        distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (InChaseRange())
        {
            EngageDruid();
        }
        //else if (InFlowerRange())
        //{
        //    EngageFlowers();
        //}

        else
        {
            Roaming();
            //print("Cancel");
            // Choose the next destination point when the agent gets
            // close to the current one.
            //if (!agent.pathPending && agent.remainingDistance < 0.5f)
            //PatrolArea();
        }
    }

    private bool InChaseRange()
    {
        //distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        //return distanceToTarget < chaseRange;
        if (Vector3.Distance(transform.position, target.transform.position) < viewDistance)
        {
            Vector3 dirToPlayer = (target.transform.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);

            if (angleBetweenGuardAndPlayer < viewAngle)
            {
                return true;
            }
        }
        return false;
    }

    private void EngageFlowers()
    {
        distanceToFlower = Vector3.Distance(transform.position, flower.transform.position);
        if (distanceToFlower > agent.stoppingDistance)
        {
            agent.SetDestination(flower.transform.position);

        }
        else
        {
            //print("attack flower");
        }

    }

    private bool InFlowerRange()
    {
        distanceToFlower = Vector3.Distance(flower.transform.position, transform.position);
        return distanceToFlower < chaseRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        //print("found : " + other.tag);

        // if found a flower
        if (other.tag == "Interactable")
        {
            flower = other.gameObject;
        }
    }

    private bool InAttackRange()
    {

        distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        return distanceToTarget < chaseRange;

    }

    private void EngageDruid()
    {

        if (distanceToTarget > agent.stoppingDistance)
        {
            ChaseDurin();


        }
        else
        {
            AttackDurin();
        }

    }

    private void AttackDurin()
    {
        timeSinceLastSawPlayer = 0;
        //print("Caught");
    }

    private void ChaseDurin()
    {
        agent.SetDestination(target.transform.position);

        distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        if( distanceToTarget < transfromRange)
        {
            roboAnim.SetInteger("Robo", 1);
        }else
        {
            roboAnim.SetInteger("Robo", 2);
        }            
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }

    private Vector3 GetRoamingPos()
    {
        Vector3 randomPos = enemyPos + GetRandomDir() * UnityEngine.Random.Range(10f, 70f);
        // prevent the Robo move outside the terrain     
        randomPos = new Vector3(Mathf.Clamp(randomPos.x, minX, maxX), randomPos.y, Mathf.Clamp(randomPos.z, minZ, maxZ));
        return randomPos;
    }

    private Vector3 GetRandomDir()
    {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f)).normalized;
    }

    private void Roaming()
    {
        roboAnim.SetInteger("Robo", 2);
        // Set the agent to go to the roam position.
        agent.SetDestination(roamPos);
        float reachedPosDistance = 3f;

        // if reached roam position, generate a new roam position
        if (Vector3.Distance(transform.position, roamPos) < reachedPosDistance)
        {
            roamPos = GetRoamingPos();
        }
    }
}
