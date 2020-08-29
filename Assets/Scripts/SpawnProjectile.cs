using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile : MonoBehaviour
{
    public GameObject firePoint;
    public GameObject projectile;
    public float fireRate = 1f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= fireRate)
        {
            fireRate = Time.time + 1;
            Spawn();
        }
    }

    void Spawn()
    {
        Instantiate(projectile, firePoint.transform.position, transform.rotation);
    }
}
