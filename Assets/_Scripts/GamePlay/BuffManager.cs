using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public GameObject[] SpawnableObjects;

    Player playerController;
    PlantBehavior[] plants;
    EnemyAttack[] enemies;
    ScoreKeeperBehavior scoreKeeper;
    // Start is called before the first frame update
    void Start()
    {
        plants = FindObjectsOfType<PlantBehavior>();
        playerController = FindObjectOfType<Player>();
        enemies = FindObjectsOfType<EnemyAttack>();
        scoreKeeper = FindObjectOfType<ScoreKeeperBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void activateRandomBuff()
    {
        int selected= Random.Range(1, 5);
        
        switch (selected)
        {
            case 1:
                ActivateBuffSpeed();
                break;
            case 2:
                ActivateEnemySlow();
                break;
            case 3:
                ActivateSingSpeedBuff();
                break;
            case 4:
                SpawnWisp();
                break;
        }
           
    }
    //multi player speed
    public void ActivateBuffSpeed()
    {
        playerController.Speed = playerController.Speed * 1.3f;
        StartCoroutine(scoreKeeper.BuffMessage("Player Speed Buff: + 30%"));
    }
    //slow enemy speed 
    public void ActivateEnemySlow()
    {
        foreach (EnemyAttack enemy in enemies)
        {
            enemy.Agent.speed = enemy.Agent.speed * .9f;
        }
        StartCoroutine(scoreKeeper.BuffMessage("Enemy Slow Buff: -10%"));
    }
    //halfs time to sing to all plants
    public void ActivateSingSpeedBuff()
    {
        foreach(PlantBehavior plant in plants)
        {
            plant.TimeBetweenPhases = plant.TimeBetweenPhases / 2;
        }
        StartCoroutine(scoreKeeper.BuffMessage("Sing Speed Buff: x2"));
    }
    public void SpawnWisp()
    {
        Instantiate(SpawnableObjects[0], transform);
        StartCoroutine(scoreKeeper.BuffMessage("Wist spawn"));
    }
}
