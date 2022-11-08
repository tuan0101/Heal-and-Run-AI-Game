using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput), typeof(PlayerAudio), typeof(PlayerHealth))]
public class Player : PlayerMovement
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerMovement playerMovement;

    protected override void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();

        GetComponent<PlayerHealth>().OnDie += HandlePlayerDeath;
        GetComponent<PlayerHealth>().OnHit += HandlePlayerGetHit;

        SpawnAtRandomPos();

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerMovement();
    }

    void HandlePlayerMovement()
    {
        if (!isDead)
        //if (canMove)
        {
            movement.x = playerInput.Horizontal;
            movement.z = playerInput.Vertical;
            Vector3 moveDirection = new Vector3(movement.x * speed, 0, movement.z * speed);

            if (moveDirection != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 0.15F);

            if (movement.x != 0 || movement.z != 0)
            {
                // Runing
                anim.SetInteger("PlayerInt", 2);
            }
            else
            {
                anim.SetInteger("PlayerInt", 1);
            }
        }
    }

    void HandlePlayerDeath()
    {
        playerMovement.AnimateDead();
        Invoke("GameOver", 2f);
    }

    void HandlePlayerGetHit()
    {
        playerMovement.AnimateGetHit();
    }

    void GameOver()
    {
        SceneManager.LoadScene("DefeatScene");
    }

    void SpawnAtRandomPos()
    {
        //spawn at a random position each time the game start
        transform.position *= UnityEngine.Random.Range(0.5f, 1.5f);
    }
}
