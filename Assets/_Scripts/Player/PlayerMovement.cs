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

    public void AnimateSing()
    {
        // Singing Animation
        anim.SetBool("isSing", true);
    }

}
