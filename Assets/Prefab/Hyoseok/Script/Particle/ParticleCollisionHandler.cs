using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using BS.Player;
namespace BS.Particle
{
    public class ParticleCollisionHandler : MonoBehaviour
    {
        PlayerHealth playerHealth;
        private new ParticleSystem particleSystem;
        private bool hasCollided = false;
        //패턴별데미지
        //public int damageAmount = 10;
      
        void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();
        }

        void OnParticleCollision(GameObject other)
        {
            if (!hasCollided && other.CompareTag("Player"))
            {
                hasCollided = true; Debug.Log($"other = {other}");
            }

            // 플레이어에게 데미지를 줍니다.
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                //playerHealth.TakeDamage(damageAmount);
            }
            StartCoroutine(ResetCollision());
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

        //공격받는 딛레이
        IEnumerator ResetCollision()
        {
            yield return new WaitForSeconds(0.5f);
            hasCollided = false ;
        }
    }


}