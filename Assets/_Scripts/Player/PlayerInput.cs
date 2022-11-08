using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public float Sing { get; private set; }

    public event Action OnSing = delegate { };
    public event Action OffSing = delegate { };

    // Update is called once per frame
    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
        Sing = Input.GetAxisRaw("Interact");
        if (Sing != 0) OnSing();
        else OffSing();
    }
}
