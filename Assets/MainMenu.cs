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

    static public int level = 1;

    // Start is called before the first frame update
    void Start()
    {
        //print(dropdownMenu.value);
        dropdownMenu = dropdownMenu.GetComponent<Dropdown>();
        dropdownMenu.onValueChanged.AddListener(delegate
        {
            level = dropdownMenu.value + 1;
        });
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayHover()
    {
        hoverSound.GetComponent<AudioSource>().Play();
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main Scene");
    }



}
