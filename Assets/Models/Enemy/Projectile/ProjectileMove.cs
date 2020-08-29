using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    public float speed;
    public GameObject muzzlePrefab;
    public GameObject hitPrefab;

    float lifeDuration = 2;

    // Start is called before the first frame update
    void Start()
    {
        if (muzzlePrefab)
        {
            var muzzleVFX = Instantiate(muzzlePrefab, transform.position, Quaternion.identity);
            muzzleVFX.transform.forward = gameObject.transform.forward;

            //var psMuzzle = muzzleVFX.GetComponent<ParticleSystem>();
            //if (psMuzzle)
            //{
            //    print("muzzle");
            //    Destroy(muzzleVFX, psMuzzle.main.duration);
            //}
            //else
            //{
                var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(muzzleVFX, psChild.main.duration);
            //}
        }
        // disappear after 2s
        Destroy(this.gameObject, lifeDuration);
    }

    // Update is called once per frame
    void Update()
    {
        if(speed != 0)
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        speed = 0;
        ContactPoint contactPoint = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contactPoint.normal);
        Vector3 pos = contactPoint.point;
        if (hitPrefab)
        {
            var hitVFX = Instantiate(hitPrefab, pos, rot);

            //var psHit = hitVFX.GetComponent<ParticleSystem>();
            //if (psHit)
            //{
            //    Destroy(hitVFX, psHit.main.duration);
            //}
            //else
            //{
                var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitVFX, psChild.main.duration);
            //}
        }
        Destroy(this.gameObject);
    }
}
