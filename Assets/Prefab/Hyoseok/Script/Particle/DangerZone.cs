using BS.Player;
using UnityEngine;

public class DangerZone : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (!enabled) return;
        if (other.CompareTag("Player"))
        {
            // 플레이어에게 데미지 적용
         
            PlayerHealth playerHealth = other.GetComponentInChildren<PlayerHealth>();
            if (playerHealth != null)//&& !playerHealth.IsInvincible 무적이아닐때 나중에 추가
            {
                playerHealth.TakeDamage(99999f, false);
                Debug.Log("플레이어 사망");
            }
        }
    }
}
