using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    PlayerController playerController;
    PlantBehavior[] plants;
    public GameObject[] SpawnableObjects;
    private MonsterAI[] monsters;
    // Start is called before the first frame update
    void Start()
    {
        plants = FindObjectsOfType<PlantBehavior>();
        playerController = FindObjectOfType<PlayerController>();
        monsters = FindObjectsOfType<MonsterAI>();
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
        Debug.Log("Speed Buff");
    }
    //slow enemy speed 
    public void ActivateEnemySlow()
    {
        foreach (MonsterAI monster in monsters)
        {
            monster.agent.speed = monster.agent.speed * .9f;
        }
       
        Debug.Log("Enemy Slow buff empty");
    }
    //halfs time to sing to all plants
    public void ActivateSingSpeedBuff()
    {
        foreach(PlantBehavior plant in plants)
        {
            plant.TimeBetweenPhases = plant.TimeBetweenPhases / 2;
        }
        Debug.Log("SingSpeedBuff");
    }
    public void SpawnWisp()
    {
        Instantiate(SpawnableObjects[0], transform);
        Debug.Log("wist spawn");
    }
}
