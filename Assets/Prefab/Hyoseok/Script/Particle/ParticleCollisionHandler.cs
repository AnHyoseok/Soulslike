using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using BS.Player;

namespace BS.Particle
{
    public class ParticleCollisionHandler : MonoBehaviour
    {
        private new ParticleSystem particleSystem;
        private HashSet<GameObject> collidedObjects = new HashSet<GameObject>();
        // ���Ϻ� ������
        public int damageAmount = 10;

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
                Debug.Log("�÷��̾�ã��");

                // �ڽ� ��ü���� PlayerHealth ������Ʈ ã��
                PlayerHealth playerHealth = other.GetComponentInChildren<PlayerHealth>();
                if (playerHealth != null)
                {
                    Debug.Log($"{damageAmount}��ŭ ������ ����");
                    playerHealth.TakeDamage(damageAmount, false);
                    Debug.Log($"hp={playerHealth.CurrentHealth}");
                    
                }
            }
        }



    }
}
