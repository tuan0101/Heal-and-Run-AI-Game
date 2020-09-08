using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject hoverSound;
    [SerializeField] Dropdown dropdownMenu;
    GameObject winLevel;
    static public int level = 1;

    // Start is called before the first frame update
    void Start()
    {
        dropdownMenu.value = level - 1;
        
        dropdownMenu = dropdownMenu.GetComponent<Dropdown>();
        dropdownMenu.onValueChanged.AddListener(delegate
        {
            level = dropdownMenu.value + 1;
        });
        print(dropdownMenu.value);

        winLevel = GameObject.Find("WinLevel");
        if (winLevel)
        {
            winLevel.GetComponent<Text>().text = "You reached level " + level;
        }

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayHover()
    {
        hoverSound.GetComponent<AudioSource>().Play();
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadNextLevel()
    {
        print("scene ++");
        level++;
        dropdownMenu.options.Add(new Dropdown.OptionData() { text = "level " + level });
        SceneManager.LoadScene("MainScene");
    }
}
