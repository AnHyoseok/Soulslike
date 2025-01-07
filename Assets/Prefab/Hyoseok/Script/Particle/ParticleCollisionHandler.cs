using System.Collections.Generic;
using UnityEngine;

namespace BS.Particle
{
    public class ParticleCollisionHandler : MonoBehaviour
    {
       
            private new ParticleSystem particleSystem;
            public int damageAmount = 10;

            void Start()
            {
                particleSystem = GetComponent<ParticleSystem>();
            }

            void OnParticleCollision(GameObject other)
            {
                //if (other.CompareTag("Player"))
                //{
                //    // 플레이어에게 데미지를 줍니다.
                //    PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                //    if (playerHealth != null)
                //    {
                //        playerHealth.TakeDamage(damageAmount);
                //    }

                //    // 플레이어와 충돌한 파티클 비활성화
                //    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
                //    particleSystem.GetParticles(enter);
                //    for (int i = 0; i < enter.Count; i++)
                //    {
                //        ParticleSystem.Particle p = enter[i];
                //        if (p.position == other.transform.position)
                //        {
                //            p.remainingLifetime = 0;
                //            enter[i] = p;
                //        }
                //    }
                //    particleSystem.SetParticles(enter.ToArray(), enter.Count);
                //}
                // if (other.CompareTag("Wall"))
                //{
                //    // 벽에 닿은 파티클 비활성화
                //    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
                //    particleSystem.GetParticles(enter);
                //    for (int i = 0; i < enter.Count; i++)
                //    {
                //        ParticleSystem.Particle p = enter[i];
                //        if (p.position == other.transform.position)
                //        {
                //            p.remainingLifetime = 0;
                //            enter[i] = p;
                //        }
                //    }
                //    particleSystem.SetParticles(enter.ToArray(), enter.Count);
                //}
            }
        }

    
}