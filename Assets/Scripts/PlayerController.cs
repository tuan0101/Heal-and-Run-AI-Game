using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 50f;

    bool singing = false;
    public GameObject ParticalManager;
    public AudioClip[] songs;
    AudioSource audioSource;
    public GameObject singRange;
    [HideInInspector]
    public ParticalManagerBehavior managerBehavior;

    Animator anim;
    GameObject obj;
    Vector3 Movement;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        managerBehavior = ParticalManager.GetComponent<ParticalManagerBehavior>();

        obj = GameObject.Find("PlayerBody");
        anim = obj.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        Movement.x = Input.GetAxisRaw("Horizontal");
        Movement.z = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = new Vector3(Movement.x * speed, 0, Movement.z * speed);
        //
        //Vector3 movement = new Vector3(Movement.x, 0.0f, Movement.z);
        if (moveDirection != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 0.15F);
        
        //transform.Translate(movement, Space.Self);
        //transform.position += moveDirection * Time.deltaTime;




        if (Movement.x != 0 || Movement.z != 0)
        {
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }
        sing();
    }
    private void FixedUpdate()
    {
        if(!singing)
        {
            
            rb.MovePosition(rb.position + Movement * speed * Time.deltaTime);
        }
       
    }
    private void sing()
    {
        
        if (Input.GetAxisRaw("Interact") != 0)
        {
            if(singing == false)
            {
                audioSource.Play();
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
            anim.SetBool("isSing", false);
        }
       
    }

    // Make this parent always look at the child rotation

    
}
