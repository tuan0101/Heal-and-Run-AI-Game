using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadScene(int sceneBuildIndex)
    {

        // SceneManager.LoadScene(0);
        SceneManager.LoadScene(sceneBuildIndex);
    }


    public void quitGame()
    {
        Application.Quit();
    }
}
