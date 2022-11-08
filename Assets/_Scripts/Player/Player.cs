using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput), typeof(PlayerAudio), typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerAudio playerAudio;
    [SerializeField] private PlayerMovement playerMovement;
    public GameObject ParticalManager;

    bool isDead = false;
    ParticalManagerBehavior managerBehavior;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAudio = GetComponent<PlayerAudio>();
        playerMovement = GetComponent<PlayerMovement>();

        managerBehavior = ParticalManager.GetComponent<ParticalManagerBehavior>();

        GetComponent<PlayerHealth>().OnDie += HandlePlayerDeath;
        GetComponent<PlayerHealth>().OnHit += HandlePlayerGetHit;
        //spawn at a random position each time the game start
        transform.position *= UnityEngine.Random.Range(0.5f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
