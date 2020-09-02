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

    Renderer rend; 
    Material myMat, currentMat;
    public Material liveMat, deadMat;
    public GameObject childTree;

    int myHP = 75;


    void Start()
    {
        scoreKeeper = FindObjectOfType<ScoreKeeperBehavior>();

        rend = childTree.GetComponent<Renderer>();
        currentMat = rend.material;
        //rend.enabled = true;
        rend.sharedMaterial = liveMat;

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
    void OnTriggerEnter(Collider other)
    {
        // activate if the if in range
        if(other.gameObject.CompareTag("SingSphere"))
        {
            Activate();
            other.gameObject.GetComponentInParent<PlayerController>().managerBehavior.target = this.gameObject;
            Invoke("CurrenForm", 1);
        }



    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("SingSphere"))
        {
            if(!complete)
            {
                Deactivate();

            }
            
        } 
        
    }

    void OnCollisionEnter(Collision collision)
    {
        //update HP when being attacked
        if (collision.collider.tag == "projectile")
        {
            myHP -= 25;
            if (myHP <= 0)
                DeadForm();
        }
    }

    // Transform to a dead tree
    void DeadForm()
    {
        this.tag = "dead";
        rend.sharedMaterial = deadMat;
    }

    // Transform to a live tree
    void CurrenForm()
    {
        rend.sharedMaterial = currentMat;
    }

}
