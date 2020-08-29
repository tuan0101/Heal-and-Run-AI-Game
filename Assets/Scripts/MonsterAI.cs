using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{

    [SerializeField] GameObject terrain;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject firePoint;

    GameObject target;
    GameObject flower;

    MeshCollider col;

    public float viewDistance; 
    public Light spotLight;

    [HideInInspector]
    public NavMeshAgent agent;

    // For chasing
    [SerializeField] float chaseRange = 50f;
    float attackRange = 8f;
    float fireRate = 1f;
    float fireDelay = 1f;
    float transfromRange = 13f;
    float distanceToTarget = Mathf.Infinity;
    float distanceToFlower = Mathf.Infinity;
    float minX, maxX, minZ, maxZ;
    float viewAngle;

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
        

        roboAnim = GetComponentInChildren<Animator>();

        col = terrain.GetComponent<MeshCollider>();

        // cannot call col.bounds in update()
        minX = col.bounds.min.x;
        maxX = col.bounds.max.x;
        minZ = col.bounds.min.z;
        maxZ = col.bounds.max.z;
        print(minX);
        print(roamPos);
        roamPos = GetRoamingPos();
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

        }
    }

    private bool InChaseRange()
    {
        distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        // attack if in range
        if (distanceToTarget < attackRange)
        {
            return true;
        }

        // else chase if in view
        if (Vector3.Distance(transform.position, target.transform.position) < viewDistance)
        {
            Vector3 dirToPlayer = (target.transform.position - transform.position).normalized;
            return TargetInView();
        }
        return false;
    }

    bool TargetInView() {
        Vector3 dirToPlayer = (target.transform.position - transform.position).normalized;
        float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);

        if (angleBetweenGuardAndPlayer < viewAngle)
            return true;
        else
            return false;
    }

    private void EngageDruid()
    {
        
        //if (distanceToTarget > agent.stoppingDistance)
        if (distanceToTarget > attackRange)
        {
            ChaseDruid();
        }
        else
        {
            AttackDruid();
        }

    }

    private void AttackDruid()
    {
        roboAnim.SetInteger("Robo", 1);
        //print("attk");
        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.03f);
        //transform.rotation = Quaternion.Slerp(transform.rotation, target.transform.rotation, 0.03f);
        if (TargetInView())
        {
            if (Time.time >= fireRate)
            {
                fireRate = Time.time + fireDelay;
                Instantiate(projectilePrefab, firePoint.transform.position, transform.rotation);
            }
        }


    }

    private void ChaseDruid()
    {
        agent.SetDestination(target.transform.position);

        distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        if (distanceToTarget < transfromRange)
        {
            roboAnim.SetInteger("Robo", 1);
        }
        else
        {
            roboAnim.SetInteger("Robo", 2);
        }

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



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }

    private Vector3 GetRoamingPos()
    {
        print("direction: " + GetRandomDir());
        Vector3 randomPos = enemyPos + GetRandomDir() * UnityEngine.Random.Range(10f, 70f);
        print(randomPos);
        // prevent the Robo move outside the terrain     
        randomPos = new Vector3(Mathf.Clamp(randomPos.x, minX, maxX), randomPos.y, Mathf.Clamp(randomPos.z, minZ, maxZ));
        print(minX);
        print(randomPos);
        return randomPos;
    }

    private Vector3 GetRandomDir()
    {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f)).normalized;
    }

    private void Roaming()
    {
        //print("roaming");
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
