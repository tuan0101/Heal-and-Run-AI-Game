using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispBehaviour : MonoBehaviour
{
    GameObject player;
    [SerializeField] float orbitRange =.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        int number = Random.Range(-1, 2);
        if(number == 0)
        {
            number = 1;
        }
        Vector3 relativePos = (player.transform.position + new Vector3(0,.5f, 0)) - transform.position;
        Quaternion rotation =  Quaternion.LookRotation(relativePos * number);
        
        Quaternion current = transform.localRotation;

        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime);
        transform.Translate(0, 0, orbitRange * Time.deltaTime);
    }
}
