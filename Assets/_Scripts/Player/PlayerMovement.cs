using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    bool canMove = true;
    public float speed = 50f;
    bool singing = false;

    Rigidbody rb;
    Animator anim;
    GameObject obj;
    Vector3 Movement;
    public GameObject ParticalManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        obj = GameObject.Find("PlayerBody");
        anim = obj.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void sing2()
    {
        if (Input.GetAxisRaw("Interact") != 0)
        //if (Input.GetKeyDown(KeyCode.F))
        {
            if (singing == false)
            {
                audioSource.Play();
                // Singing Animation
                anim.SetBool("isSing", true);
            }
            singRange.SetActive(true);
            singing = true;
            ParticalManager.SetActive(true);
        }
        else
        {
            audioSource.clip = songs[Random.Range(0, songs.Length)];
            audioSource.Stop();
            singing = false;
            singRange.SetActive(false);
            ParticalManager.SetActive(false);
            // Do nothing
            anim.SetBool("isSing", false);
        }
    }

    void Sing()
    {
        if (singing == false)
        {
            // Singing Animation
            anim.SetBool("isSing", true);
        }
    }

}
