using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreKeeperBehavior : MonoBehaviour
{
    public BuffManager buffManager;
    int obtainTrees=0;
    public Text obtainText;
    int remainTrees;
    public Text remainText;

    public Text currentLevel;
    LevelGenerator generator;
    // Start is called before the first frame update
    void Start()
    {
        buffManager = FindObjectOfType<BuffManager>();
        generator = FindObjectOfType<LevelGenerator>();
        currentLevel.text = "Level: " + MainMenu.level;

        DisplayObtainText(obtainTrees.ToString());

        remainTrees = generator.numOfTrees;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void plantHealed()
    {
        buffManager.activateRandomBuff();
        obtainTrees++;
        DisplayObtainText(obtainTrees.ToString());
        if (obtainTrees >= 10)
        {
            LoadScene("WinScene");
        }
    }

    public void plantDead()
    {
        remainTrees--;
        remainText.text = "Remain: " + remainTrees;
    }
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    void DisplayObtainText(string numString)
    {
        obtainText.text = "Obtain: " + numString + "/10";
    }

}
