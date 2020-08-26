using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreKeeperBehavior : MonoBehaviour
{
    public BuffManager buffManager;
    public int PlantsRemaining;
    public Text PlantRemainingText;
    // Start is called before the first frame update
    void Start()
    {

        PlantRemainingText.text = PlantsRemaining.ToString();
        buffManager = FindObjectOfType<BuffManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void plantHealed()
    {
        buffManager.activateRandomBuff();
        PlantsRemaining--;
        if(PlantsRemaining <= 0)
        {
            win();
        }
        PlantRemainingText.text = "Plants Remaining :" + PlantsRemaining.ToString();
    }
    public void win()
    {
        SceneManager.LoadScene(2);
    }
    public void die()
    {
        SceneManager.LoadScene(3);
    }
}
