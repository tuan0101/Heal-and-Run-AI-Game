using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 50f;
    int myHP = 100;
    bool canMove = true;
    bool isDead = false;

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
        print("main scene: " + MainMenu.level);
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
            // Runing
            anim.SetInteger("PlayerInt", 2);
        }
        else
        {
            if (isDead == false)
                anim.SetInteger("PlayerInt", 1);
        }
        sing();
    }
    private void FixedUpdate()
    {
        if(!singing && canMove)
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

    // getting attack from the enemies
    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.collider.tag == "projectile")
        {
            canMove = false;
            myHP -= 25;
            if (myHP <= 0)
            {
                print(myHP);
                print("is ded");
                // dead animation
                anim.SetTrigger("isDead");
                isDead = true;
                this.gameObject.tag = "Untagged";
            }
            else
            {
                // Got-hit animation
                anim.SetTrigger("GetHit");
                Invoke("afterGothit", 0.2f);
            }
                
            
            
            
        }
        
    }

    // Wait for 0.2s after getting a hit
    // use for Invoke function
    void afterGothit() 
    {  
        canMove = true;
    }


}
