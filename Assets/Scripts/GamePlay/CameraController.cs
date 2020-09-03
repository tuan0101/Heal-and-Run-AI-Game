using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    float start_z = -10f;
    void Start()
    {
        //x = transform.position.x;
        //y = transform.position.x;
        //z = transform.position.x;
        //print(x);
        //print(y);
        //print(z);

    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(target.position.x + x, transform.position.y + y, target.position.z +z);
        transform.position = new Vector3(target.position.x, transform.position.y, target.position.z + start_z);
    }
}
