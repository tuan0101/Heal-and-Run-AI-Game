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

    GameObject Druid;
    [SerializeField] GameObject flower;

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
        Druid = GameObject.FindWithTag("Player");
        //flower = GameObject.FindWithTag("Interactable");
        viewAngle = spotLight.spotAngle;
        enemyPos = this.transform.position;

        roboAnim = GetComponentInChildren<Animator>();

        col = terrain.GetComponent<MeshCollider>();

        // Note: cannot call col.bounds in update()
        minX = col.bounds.min.x;
        maxX = col.bounds.max.x;
        minZ = col.bounds.min.z;
        maxZ = col.bounds.max.z;
        roamPos = GetRoamingPos();
    }


    // Update is called once per frame
    void Update()
    {
        distanceToTarget = Vector3.Distance(transform.position, Druid.transform.position);

        if (InChaseRange() && Druid.tag == "Player")
        {
            EngageDruid();

        }
        else if (InFlowerRange())
        {
            EngageFlowers();
        }

        else
        {
            Roaming();

        }


    }

    private bool InChaseRange()
    {
        distanceToTarget = Vector3.Distance(Druid.transform.position, transform.position);
        // attack if in range
        if (distanceToTarget < attackRange)
        {
            return true;
        }

        // else chase if in view
        if (Vector3.Distance(transform.position, Druid.transform.position) < viewDistance)
        {
            Vector3 dirToPlayer = (Druid.transform.position - transform.position).normalized;
            return TargetInView(Druid);
        }
        return false;
    }

    bool TargetInView(GameObject obj) {
        Vector3 dirToPlayer = (obj.transform.position - transform.position).normalized;
        float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);

        if (angleBetweenGuardAndPlayer < viewAngle)
            return true;
        else
            return false;
    }

    private void EngageDruid()
    {
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
        Attack(Druid);

    }

    void Attack(GameObject obj)
    {
        print("attack");
        // rotate to face the target
        Quaternion targetRotation = Quaternion.LookRotation(obj.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.03f);

        if (TargetInView(obj))
        {

            if (Time.time >= fireRate)
            {
                // fire
                roboAnim.SetInteger("Robo", 0);
                StartCoroutine(StopMoving());
                fireRate = Time.time + fireDelay;
                Instantiate(projectilePrefab, firePoint.transform.position, transform.rotation);
            }
            agent.enabled = true;
        }

    }

    IEnumerator StopMoving(){
        agent.enabled = false;
        yield return new WaitForSeconds(0.5f);
        agent.enabled = true;
    }


    private void ChaseDruid()
    {
        agent.SetDestination(Druid.transform.position);

        distanceToTarget = Vector3.Distance(Druid.transform.position, transform.position);
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
            Attack(flower);
        }

    }

    private bool InFlowerRange()
    {
        distanceToFlower = Vector3.Distance(flower.transform.position, transform.position);
        return distanceToFlower < chaseRange;
    }

    // if found a flower, set it a target
    void OnTriggerEnter(Collider other)
    {
        
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
        // running animation
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
