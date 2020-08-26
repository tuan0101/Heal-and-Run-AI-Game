using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBehavior : InteractableBehavior
{
    public GameObject[] ParticalPhases;
    //the current punch partical
    public GameObject CurrentPhase;
    public float TimeBetweenPhases;
    private float holderTime;
    private ScoreKeeperBehavior scoreKeeper;

    
    
    // Start is called before the first frame update
    void Start()
    {
        scoreKeeper = FindObjectOfType<ScoreKeeperBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!complete)
        { 
            if(beingSungTo)
            {
                timeElapsed();           
            }
            ChargeBarAnimator();

        }

      
    }

    public override void Activate()
    {
        holderTime = Time.time;
        beingSungTo = true;
        
    }
    public override void Deactivate()
    {
        beingSungTo = false;
        CurrentPhase = ParticalPhases[0];
    }
    void timeElapsed()
    {  
        float timeElapsed = Time.time - holderTime;
      
        if (timeElapsed >= 2 * TimeBetweenPhases)
        {
            complete = true;
            scoreKeeper.plantHealed();
            CurrentPhase = ParticalPhases[3];            
        }
        else if (timeElapsed >= 1 * TimeBetweenPhases)
        {
            CurrentPhase = ParticalPhases[2];          
        }
        else if (timeElapsed >= 0 * TimeBetweenPhases)
        {
            CurrentPhase = ParticalPhases[1];           
        }
        else
        {
            CurrentPhase = ParticalPhases[0];          
        }
    }
    void ChargeBarAnimator()
    {
        foreach (GameObject i in ParticalPhases)
        {
            if (i != CurrentPhase)
            {
                i.SetActive(false);
            }
        }
        CurrentPhase.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("SingSphere"))
        {
            print("found the player");
            Activate();
            other.gameObject.GetComponentInParent<PlayerController>().managerBehavior.target = this.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("SingSphere"))
        {
            if(!complete)
            {
                Deactivate();

            }
            
        } 
        
    }

}
