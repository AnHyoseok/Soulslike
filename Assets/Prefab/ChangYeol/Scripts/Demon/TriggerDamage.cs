using BS.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BS.Demon
{
    public class TriggerDamage : MonoBehaviour
    {
        #region Variables
        private HashSet<GameObject> damagedObjects = new HashSet<GameObject>();
        public int damageAmount = 10;
        #endregion
        private void Start()
        {
            StartCoroutine(TriggerOn());
        }
        void OnTriggerEnter(Collider other)
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                Debug.Log("플레이어 발견!");

                // 자식 객체에서 PlayerHealth 컴포넌트를 찾음
                PlayerHealth playerHealth = other.GetComponentInChildren<PlayerHealth>();
                if (playerHealth != null && !damagedObjects.Contains(other.gameObject))
                {
                    Debug.Log($"{damageAmount}만큼 데미지 입음");
                    playerHealth.TakeDamage(damageAmount, false);
                    damagedObjects.Add(other.gameObject);
                    StartCoroutine(ResetCollision(other.gameObject));
                }
            }
            StopCoroutine(TriggerOn());
        }

        // 일정 시간 후 충돌 정보 리셋
        IEnumerator ResetCollision(GameObject other)
        {
            yield return new WaitForSeconds(0.5f);
            damagedObjects.Remove(other);
        }
        IEnumerator TriggerOn()
        {
            if(this.GetComponent<BoxCollider>())
            {
                this.GetComponent<BoxCollider>().enabled = false;
                yield return new WaitForSeconds(0.5f);
                this.GetComponent<BoxCollider>().enabled = true;
                yield return new WaitForSeconds(0.5f);
                this.GetComponent<BoxCollider>().enabled = false;
            }
            else if (this.GetComponent<SphereCollider>())
            {
                this.GetComponent<SphereCollider>().enabled = false;
                yield return new WaitForSeconds(0.5f);
                this.GetComponent<SphereCollider>().enabled = true;
                yield return new WaitForSeconds(0.5f);
                this.GetComponent<SphereCollider>().enabled = false;
            }
        }
    }
}