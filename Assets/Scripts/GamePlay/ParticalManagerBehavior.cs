using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticalManagerBehavior : MonoBehaviour
{
        ParticleSystem ps;
        ParticleSystem.Particle[] m_Particles;
        public GameObject target;
        public float speed = 5f;
        int numParticlesAlive;
        void Start()
        {
            ps = GetComponent<ParticleSystem>();
            //if (!GetComponent<Transform>())
            //{
            //    GetComponent<Transform>();
            //}
        }
        void Update()
        {
            if(target != null)
            {
                m_Particles = new ParticleSystem.Particle[ps.main.maxParticles];
                numParticlesAlive = ps.GetParticles(m_Particles);
                float step = speed * Time.deltaTime;
                Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y + 2f, target.transform.position.z);
                for (int i = 0; i < numParticlesAlive; i++)
                {
                    m_Particles[i].position = Vector3.LerpUnclamped(m_Particles[i].position, targetPos, step);
                }
                ps.SetParticles(m_Particles, numParticlesAlive);  

                if(Vector3.Distance(this.transform.position,target.transform.position) > 9)
                {
                  target = null;
                }

            }
   
        }      
}
