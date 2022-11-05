using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBehavior : MonoBehaviour
{
    [HideInInspector]
    public bool beingSungTo = false;
    public bool complete = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {

    }
    public virtual void Activate()
    {
        Debug.Log("No activation");
    }
    public virtual void Deactivate()
    {
        Debug.Log("No Deactivate");
    }
    public virtual void AltActivation()
    {
        Debug.Log("no AltActivation");
    }

}
