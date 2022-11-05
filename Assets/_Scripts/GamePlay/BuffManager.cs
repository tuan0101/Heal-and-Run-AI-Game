using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public GameObject[] SpawnableObjects;

    PlayerController playerController;
    PlantBehavior[] plants;
    MonsterAI[] monsters;
    ScoreKeeperBehavior scoreKeeper;
    // Start is called before the first frame update
    void Start()
    {
        plants = FindObjectsOfType<PlantBehavior>();
        playerController = FindObjectOfType<PlayerController>();
        monsters = FindObjectsOfType<MonsterAI>();
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
        playerController.speed = playerController.speed * 1.3f;
        StartCoroutine(scoreKeeper.BuffMessage("Player Speed Buff: + 30%"));
    }
    //slow enemy speed 
    public void ActivateEnemySlow()
    {
        foreach (MonsterAI monster in monsters)
        {
            monster.agent.speed = monster.agent.speed * .9f;
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
