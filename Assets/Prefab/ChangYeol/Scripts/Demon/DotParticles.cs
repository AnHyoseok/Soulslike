using BS.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BS.Particle
{
    public class DotParticles : MonoBehaviour
    {
        #region Variables
        private new ParticleSystem particleSystem;
        private HashSet<GameObject> collidedObjects = new HashSet<GameObject>();
        // 패턴별 데미지
        public int maxDamage = 1;
        public int minDamage = 2;
        #endregion
        void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();
        }

        void OnParticleCollision(GameObject other)
        {
            Debug.Log(other.name);
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                Debug.Log("플레이어찾음");

                // 자식 객체에서 PlayerHealth 컴포넌트 찾기
                PlayerHealth playerHealth = other.GetComponentInChildren<PlayerHealth>();
                if (playerHealth != null)
                {
                    int damage = Random.Range(minDamage, maxDamage);
                    Debug.Log($"{damage}만큼 데미지 입음");
                    playerHealth.TakeDamage(damage, false);
                    Debug.Log($"hp={playerHealth.CurrentHealth}");
                }
            }
        }
    }
}