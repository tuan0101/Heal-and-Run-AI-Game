using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject firePoint;
    public float chaseRange = 20f;
    public float transfromRange = 13f;

    public float attackRange = 8f;
    public float fireRate = 1f;
    public float fireDelay = 1f;

    //enemy vision
    public float viewAngle;
    public float viewDistance;

    public Light spotLight;
    public Animator roboAnim;

    public NavMeshAgent agent;
    int difLV; // difficult level

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        roboAnim = GetComponentInChildren<Animator>();

        viewAngle = spotLight.spotAngle - 10f;

        SetLevelDifficulty();
    }

    public void Attack(GameObject obj)
    {
        roboAnim.SetInteger("Robo", 1);

        // rotate to face the target
        Quaternion targetRotation = Quaternion.LookRotation(obj.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.03f);

        if (TargetInView(obj))
        {
            if (Time.time >= fireRate)
            {
                // fire
                StartCoroutine(StopMoving());
                roboAnim.SetInteger("Robo", 0);
                fireRate = Time.time + fireDelay;
                Invoke("Fire", 0.5f);
            }
            agent.enabled = true;
        }
    }

    bool TargetInView(GameObject obj)
    {
        Vector3 dirToPlayer = (obj.transform.position - transform.position).normalized;
        float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);

        if (angleBetweenGuardAndPlayer < viewAngle)
            return true;
        else
            return false;
    }

    IEnumerator StopMoving()
    {
        agent.enabled = false;
        yield return new WaitForSeconds(0.5f);
        agent.enabled = true;
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
        chaseRange += difLV * 2f;
        attackRange += difLV / 5f;
        transfromRange += difLV / 5f;
        fireDelay -= difLV / 25f;
        viewAngle += difLV;
        agent.speed += difLV / 2f;
    }
}
