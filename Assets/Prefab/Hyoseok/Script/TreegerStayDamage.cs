using BS.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BS.vampire { 
public class TreegerStayDamage : MonoBehaviour
{
        #region Variables
        private HashSet<GameObject> damagedObjects = new HashSet<GameObject>();
        public int damageAmount = 10;
        #endregion
        void OnTriggerStay(Collider other)
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                Debug.Log("�÷��̾� �߰�!");

                // �ڽ� ��ü���� PlayerHealth ������Ʈ�� ã��
                PlayerHealth playerHealth = other.GetComponentInChildren<PlayerHealth>();
                if (playerHealth != null && !damagedObjects.Contains(other.gameObject))
                {
                    Debug.Log($"{damageAmount}��ŭ ������ ����");
                    playerHealth.TakeDamage(damageAmount, false);
                    Debug.Log($"hp={playerHealth.CurrentHealth}");
                    damagedObjects.Add(other.gameObject);
                    StartCoroutine(ResetCollision(other.gameObject));
                }
            }
        }

        // ���� �ð� �� �浹 ���� ����
        IEnumerator ResetCollision(GameObject other)
        {
            yield return new WaitForSeconds(0.5f);
            damagedObjects.Remove(other);
        }
    }
}