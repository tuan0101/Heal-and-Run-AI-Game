using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAttack), typeof(Terrain), typeof(EnemyMovement))]
public class EnemyAI : EnemyMovement
{
    [SerializeField] GameObject flower;

    // Update is called once per frame
    void Update()
    {
        if (InChaseRange())
        {
            EngagePlayer();

        }
        else if (InFlowerRange())
        {
            EngageFlowers();
        }

        else
        {
            Roaming();
            //Invoke("printR", 3f);
        }
    }

    private void EngagePlayer()
    {
        if (distanceToTarget > enemyAttack.AttackRange)
        {
            ChasePlayer();
        }
        else
        {
            AttackPlayer();
        }

    }

    float DistanceToTarget(GameObject obj)
    {
        return Vector3.Distance(transform.position, obj.transform.position);
    }

    // 2 cases: either attack or chase within the Chase Range
    bool InChaseRange()
    {
        if (player)
        {
            // if in attk range => immediately attack the player
            distanceToTarget = DistanceToTarget(player);
            if (distanceToTarget < enemyAttack.AttackRange) return true;
            // else, check if the player is in this enemy's view
            // (enemy can see the player)
            if (distanceToTarget < enemyAttack.ViewDistance)
            {
                Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
                return TargetInView(player);
            }
        }
        return true;
    }

    bool TargetInView(GameObject obj)
    {
        Vector3 dirToPlayer = (obj.transform.position - transform.position).normalized;
        float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);

        if (angleBetweenGuardAndPlayer < enemyAttack.ViewAngle)
            return true;
        else
            return false;
    }

    private void EngageFlowers()
    {
        if (flower)
        {
            distanceToFlower = DistanceToTarget(flower);
            if (distanceToFlower > enemyAttack.Agent.stoppingDistance)
            {
                enemyAttack.Agent.SetDestination(flower.transform.position);

            }
            else
            {
                if (flower.tag == "Interactable")
                    enemyAttack.Attack(flower);
                else
                    // disconect with the flower to target another 
                    flower = null;
            }
        }
    }

    // if found a flower, set it a target
    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Interactable")
        {
            flower = other.gameObject;
        }
    }

    private bool InFlowerRange()
    {
        if (flower)
        {
            distanceToFlower = Vector3.Distance(flower.transform.position, transform.position);
        }
        else
        {
            return false;
        }

        return distanceToFlower < enemyAttack.ChaseRange;
    }
}
