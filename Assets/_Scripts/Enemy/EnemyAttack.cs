using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    int difLV; // difficult level

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject firePoint;
    [SerializeField] Light spotLight;

    public float ChaseRange { get; set; } = 20f;
    public float TransfromRange { get; set; } = 13f;

    public float AttackRange { get; set; } = 8f;
    public float FireRate { get; set; } = 1f;
    public float FireDelay { get; set; } = 1f;

    //enemy vision
    public float ViewAngle { get; set; }
    public float ViewDistance { get; set; }

    public Animator RoboAnim { get; set; }

    public NavMeshAgent Agent { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        RoboAnim = GetComponentInChildren<Animator>();

        ViewAngle = spotLight.spotAngle - 10f;

        SetLevelDifficulty();
    }

    public void Attack(GameObject obj)
    {
        RoboAnim.SetInteger("Robo", 1);

        // rotate to face the target
        Quaternion targetRotation = Quaternion.LookRotation(obj.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.03f);

        if (TargetInView(obj))
        {
            if (Time.time >= FireRate)
            {
                // fire
                StartCoroutine(StopMoving());
                RoboAnim.SetInteger("Robo", 0);
                FireRate = Time.time + FireDelay;
                Invoke("Fire", 0.5f);
            }
            Agent.enabled = true;
        }
    }

    bool TargetInView(GameObject obj)
    {
        Vector3 dirToPlayer = (obj.transform.position - transform.position).normalized;
        float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);

        if (angleBetweenGuardAndPlayer < ViewAngle)
            return true;
        else
            return false;
    }

    IEnumerator StopMoving()
    {
        Agent.enabled = false;
        yield return new WaitForSeconds(0.5f);
        Agent.enabled = true;
        //agent.isStopped = true;
        //yield return new WaitForSeconds(0.5f);
        //agent.isStopped = false;
    }
    void Fire()
    {
        Instantiate(projectilePrefab, firePoint.transform.position, transform.rotation);
        //agent.enabled = true;
    }

    void SetLevelDifficulty()
    {
        difLV = MainMenu.level;
        ChaseRange += difLV * 2f;
        AttackRange += difLV / 5f;
        TransfromRange += difLV / 5f;
        FireDelay -= difLV / 25f;
        ViewAngle += difLV;
        Agent.speed += difLV / 2f;
    }
}
