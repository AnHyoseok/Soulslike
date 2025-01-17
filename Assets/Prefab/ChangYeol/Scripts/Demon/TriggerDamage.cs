using BS.Player;
using BS.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BS.Demon
{
    public class TriggerDamage : MonoBehaviour
    {
        #region Variables
        private HashSet<GameObject> damagedObjects = new HashSet<GameObject>();
        public Collider triggerCollider;
        public int damageAmount = 10;
        [SerializeField]private bool isstun = false;
        [SerializeField]private float Stuntime = 1;
        public GameObject StunEffect;
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
                    if (isstun)
                    {
                        StartCoroutine(PlayerStun(playerController, Stuntime));
                    }
                }
            }
            if(triggerCollider != null)
            {
                StopCoroutine(TriggerOn());
            }
        }

        // 일정 시간 후 충돌 정보 리셋
        IEnumerator ResetCollision(GameObject other)
        {
            yield return new WaitForSeconds(0.5f);
            damagedObjects.Remove(other);
        }
        IEnumerator TriggerOn()
        {
            if(triggerCollider)
            {
                triggerCollider.enabled = false;
                yield return new WaitForSeconds(0.5f);
                triggerCollider.enabled = true;
                yield return new WaitForSeconds(0.5f);
                triggerCollider.enabled = false;
            }
        }
        IEnumerator PlayerStun(PlayerController player, float Time)
        {
            player.enabled = false;
            Vector3 effectpos = new Vector3(player.gameObject.transform.position.x, StunEffect.transform.position.y ,player.gameObject.transform.position.z);
            GameObject stun = Instantiate(StunEffect, effectpos, Quaternion.identity);
            PlayerStateMachine playerState = player.GetComponent<PlayerStateMachine>();
            playerState.ChangeState(playerState.IdleState);
            Debug.Log("Stun");
            yield return new WaitForSeconds(Time);
            player.enabled = true;
            Destroy(stun, 0.2f);
            Destroy(gameObject,0.2f);
        }
    }
}