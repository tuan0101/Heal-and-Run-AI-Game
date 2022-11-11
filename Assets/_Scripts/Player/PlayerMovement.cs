using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    protected bool isDead = false;
    protected float speed = 50f;
    protected Vector3 movement;
    protected Animator anim;

    bool canMove = true;   // disable all input while getting hit from enemy
    Rigidbody rb;
    GameObject obj;


    [Header("Sing")]
    bool singing = false;
    [SerializeField] GameObject singRange;
    [SerializeField] GameObject ParticalManager;
    ParticalManagerBehavior managerBehavior;

    public event Action OnStartSing = delegate { };
    public event Action OnStopSing = delegate { };

    public ParticalManagerBehavior ManagerBehavior { get {return managerBehavior; } set { managerBehavior=value; } }

    // public get set
    public float Speed { get { return speed; } set { speed = value; } }
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        obj = GameObject.Find("PlayerBody");
        anim = obj.GetComponent<Animator>();
        managerBehavior = ParticalManager.GetComponent<ParticalManagerBehavior>();

        GetComponent<PlayerInput>().OnSing += AnimateSing;
        GetComponent<PlayerInput>().OffSing += StopSinging;
    }

    private void FixedUpdate()
    {
        if (!singing && canMove && !isDead)
        {

            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
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
