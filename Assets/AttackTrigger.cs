using BS.Player;
using UnityEngine;

namespace BS.Enemy.Set
{
    public class AttackTrigger : MonoBehaviour
    {
        private bool hitCheck;
        [SerializeField] float damage = 10;

        private void OnEnable()
        {
            hitCheck = false;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (hitCheck) return;

            // 충돌한 오브젝트의 레이어가 "Player"인지 확인
            if (other.gameObject.layer == LayerMask.NameToLayer(SetProperty.PLAYER_LAYER))
            {
                // 플레이어에게 데미지를 주는 로직 처리
                Debug.Log("플레이어에게 데미지!");
                hitCheck = true;

                //TODO : 데미지를 주고 필요시 넉백, 경직 구현
                //PlayerStats playerStats = other.GetComponent<PlayerStats>();
                //if (playerStats != null)
                //{
                //    playerStats.TakeDamage(damage); // 예시 데미지 값
                //}


            }
        }
    }
}