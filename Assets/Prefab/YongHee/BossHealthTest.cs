using UnityEngine;
using System;

public class BossHealthTest : MonoBehaviour
{
    public int maxHealth = 100; // 보스 최대 체력
    private int currentHealth;

    // 체력 변경 이벤트 (UI와 연동)
    public event Action<int, int> OnHealthChanged;

    private void Start()
    {
        currentHealth = maxHealth;
        NotifyHealthChanged();
    }

    // 대미지 처리
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 체력을 0 이상, maxHealth 이하로 유지
        Debug.Log($"Boss took {damage} damage. Current health: {currentHealth}");

        NotifyHealthChanged();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 체력 변경 알림
    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    // 보스 사망 처리
    private void Die()
    {
        Debug.Log("Boss defeated!");
        // 보스 사망 로직 추가 (예: 애니메이션, 제거)
        //Destroy(gameObject); // 보스 오브젝트 제거
    }
}
