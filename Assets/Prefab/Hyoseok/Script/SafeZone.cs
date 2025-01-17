using BS.Player;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
          
                // 자식 객체에서 PlayerHealth 컴포넌트 찾기
                PlayerHealth playerHealth = other.GetComponentInChildren<PlayerHealth>();
                if (playerHealth != null)
                {
                    //플레이어 무적만들기
                    Debug.Log("플레이어 무적");

                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {

                // 자식 객체에서 PlayerHealth 컴포넌트 찾기
                PlayerHealth playerHealth = other.GetComponentInChildren<PlayerHealth>();
                if (playerHealth != null)
                {
                    //플레이어 무적해제
                    Debug.Log("플레이어 무적해제");

                }
            }
        }
    }
}
