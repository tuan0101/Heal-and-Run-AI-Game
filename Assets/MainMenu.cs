using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject hoverSound;
    [SerializeField] Dropdown dropdownMenu;

    static public int level = 1;

    // Start is called before the first frame update
    void Start()
    {
        //print(dropdownMenu.value);
        dropdownMenu = dropdownMenu.GetComponent<Dropdown>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayHover()
    {
        hoverSound.GetComponent<AudioSource>().Play();
    }

    public void HandleDropdown()
    {
        level = dropdownMenu.value;
        print(dropdownMenu.value);
        //switch (val)
        //{
        //    case 1:
        //        print("this is 1");
        //        break;
        //}

        //if(val == 0)
        //{
        //    print("this is 0");
        //}
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main Scene");
    }

    public int getLevel()
    {
        return level;
    }
}
