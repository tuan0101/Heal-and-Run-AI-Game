using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    bool isDead = false;
    bool canMove = true;   // disable all input while getting hit from enemy
    public float speed = 50f;

    bool singing = false;
    public GameObject singRange;

    Rigidbody rb;
    Animator anim;
    GameObject obj;
    Vector3 Movement;
    public GameObject ParticalManager;

    public event Action OnStartSing = delegate { };
    public event Action OnStopSing = delegate { };
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        obj = GameObject.Find("PlayerBody");
        anim = obj.GetComponent<Animator>();

        GetComponent<PlayerInput>().OnSing += AnimateSing;
        GetComponent<PlayerInput>().OffSing += StopSinging;
    }

    private void FixedUpdate()
    {
        if (!singing && canMove && !isDead)
        {

            rb.MovePosition(rb.position + Movement * speed * Time.deltaTime);
        }
    }

    void AnimateSing()
    {
        if (singing == false)
        {
            anim.SetBool("isSing", true);
            OnStartSing();
        }
        EnableSingParticle(true);
    }

    public void StopSinging()
    {
        anim.SetBool("isSing", false);
        EnableSingParticle(false);
        OnStopSing();
    }

    public void AnimateDead()
    {
        canMove = false;
        isDead = true;
        anim.SetTrigger("Dead");
        // Deactive animation from the enemies
        this.gameObject.tag = "Untagged";
    }

    public void AnimateGetHit()
    {
        canMove = false;
        anim.SetTrigger("GetHit");
        Invoke("ReleasePlayer", 0.2f);
    }

    // Wait for 0.2s after getting a hit
    // use for Invoke function
    void ReleasePlayer()
    {
        canMove = true;
    }

    void EnableSingParticle(bool value)
    {
        singing = value;
        singRange.SetActive(value);
        ParticalManager.SetActive(value);
    }
}
