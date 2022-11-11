using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Terrain terrain;

    protected float distanceToTarget = Mathf.Infinity;
    protected float distanceToFlower = Mathf.Infinity;
    protected EnemyAttack enemyAttack;
    protected GameObject player;

    // enemy starting position
    Vector3 enemyPos;
    Vector3 roamPos;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        terrain = GetComponent<Terrain>();
        enemyAttack = GetComponent<EnemyAttack>();

        enemyPos = this.transform.position;
        roamPos = GetRoamingPos();
    }
    protected void ChasePlayer()
    {
        enemyAttack.Agent.SetDestination(player.transform.position);

        distanceToTarget = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToTarget < enemyAttack.TransfromRange)
        {
            enemyAttack.RoboAnim.SetInteger("Robo", 1);
        }
        else
        {
            enemyAttack.RoboAnim.SetInteger("Robo", 2);
        }

    }

    protected void AttackPlayer()
    {
        if (player.tag == "Player")
        {
            enemyAttack.Attack(player);
        }
        else
        {
            player = null;
        }


    }

    protected void Roaming()
    {
        // running animation
        enemyAttack.RoboAnim.SetInteger("Robo", 2);
        // Set the agent to go to the roam position.
        enemyAttack.Agent.SetDestination(roamPos);
        float reachedPosDistance = 3f;

        // if reached roam position, generate a new roam position
        if (Vector3.Distance(transform.position, roamPos) < reachedPosDistance)
        {
            roamPos = GetRoamingPos();
        }
    }

    private Vector3 GetRoamingPos()
    {
        Vector3 randomPos = enemyPos + GetRandomDir() * UnityEngine.Random.Range(10f, 70f);
        // prevent the Robo move outside the terrain     
        randomPos = terrain.ClampPosition(randomPos);

        return randomPos;
    }

    private Vector3 GetRandomDir()
    {
        return new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }
}
