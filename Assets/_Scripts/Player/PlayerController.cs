using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Un-refactor class
// was split into PlayerMovement | PlayerInput | PlayerAudio | PlayerHealth | Player
public class PlayerController : MonoBehaviour
{
    int myHP = 100;
    bool canMove = true;
    bool isDead = false;
    public float speed = 50f;
    bool singing = false;

    Rigidbody rb;
    Animator anim;
    GameObject obj;
    Vector3 Movement;
    AudioSource audioSource;

    public HealthBar healthBar;
    public GameObject ParticalManager;
    public AudioClip[] songs;  
    public GameObject singRange;
    
    [HideInInspector]
    public ParticalManagerBehavior managerBehavior;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        managerBehavior = ParticalManager.GetComponent<ParticalManagerBehavior>();

        obj = GameObject.Find("PlayerBody");
        anim = obj.GetComponent<Animator>();
       
        //spawn at a random position each time
        transform.position *= UnityEngine.Random.Range(0.5f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        //if (canMove)
        {
            Movement.x = Input.GetAxisRaw("Horizontal");
            Movement.z = Input.GetAxisRaw("Vertical");
            Vector3 moveDirection = new Vector3(Movement.x * speed, 0, Movement.z * speed);

            if (moveDirection != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 0.15F);

            if (Movement.x != 0 || Movement.z != 0)
            {
                // Runing
                anim.SetInteger("PlayerInt", 2);
            }
            else
            {
                anim.SetInteger("PlayerInt", 1);
            }
            sing();
        }

    }
    private void FixedUpdate()
    {
        if (!singing && canMove && !isDead)
        {

            rb.MovePosition(rb.position + Movement * speed * Time.deltaTime);
        }

    }
    private void sing()
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
            singRange
                .SetActive(true);
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
            // update HP bar
            healthBar.health = myHP / 100f; //scale to 1

            if (myHP <= 0)
            {
                myHP = 0;
                healthBar.health = myHP / 100f;

                // dead animation
                anim.SetTrigger("Dead");
                isDead = true;
                canMove = false;
                // Deactive animation from the enemies
                this.gameObject.tag = "Untagged";
                Invoke("GameOver", 2f);
            }
            else
            {
                // Got-hit animation
                anim.SetTrigger("GetHit");
                Invoke("afterGothit", 0.2f);
                
            }
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene("DefeatScene");
    }

    // Wait for 0.2s after getting a hit
    // use for Invoke function
    void afterGothit()
    {
        canMove = true;
    }


}
