using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{

    //Common Variables
    GameObject target;
    GameObject flower;


    //For Patrol
    public Transform[] points;
    int TransPoints = 0;
    public float viewDistance;
    public LayerMask viewMask;
    float viewAngle;
    public Light spotLight;

    [HideInInspector]
    public NavMeshAgent agent;
    //For Chase
    [SerializeField] float chaseRange = 150f;
    float distanceToTarget = Mathf.Infinity;
    float distanceToFlower = Mathf.Infinity;

    //Need Behaviors for Attack and Suspiscion
    [SerializeField] float suspiscionTime = 0f;
    float timeSinceLastSawPlayer = Mathf.Infinity;

    // enemy starting position
    Vector3 enemyPos;
    Vector3 roamPos;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("Player");
        flower = GameObject.FindWithTag("Interactable");
        viewAngle = spotLight.spotAngle;
        enemyPos = this.transform.position;
        roamPos = GetRoamingPos();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
    }


    // Update is called once per frame
    void Update()
    {
        distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (InChaseRange())
        {
            EngageDurin();
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
                
                if (!Physics.Linecast(transform.position, target.transform.position, viewMask))
                {
                    return true;
                }
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

    private void PatrolArea()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[TransPoints].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        TransPoints = (TransPoints + 1) % points.Length;
    }

    private bool InAttackRange()
    {

        distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        return distanceToTarget < chaseRange;

    }


    private void EngageDurin()
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
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }

    private Vector3 GetRoamingPos()
    {
        return enemyPos + GetRandomDir() * UnityEngine.Random.Range(10f, 70f);
    }

    private Vector3 GetRandomDir()
    {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f)).normalized;
    }

    private void Roaming()
    {
        // Set the agent to go to the roam position.
        agent.SetDestination(roamPos);
        float reachedPosDistance = 10f;

        // if reached roam position, generate a new roam position
        if (Vector3.Distance(transform.position, roamPos) < reachedPosDistance)
        {
            roamPos = GetRoamingPos();
        }
    }
}
