using UnityEngine;
using System;
using System.Collections;

public class AlienHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 1000; // 보스 최대 체력
    public float currentHealth;
    private Animator animator;
    private bool isInvincible = false; //무적여부
    //public GameObject EndingSequencerCamera; //엔딩연출카메라
    //public GameObject fadeInOut;


    public void SetInvincible(bool invincible)
    {
        isInvincible = invincible;
    }


    // 체력 변경 이벤트 (UI와 연동)
    public event Action<float, float> OnHealthChanged;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        NotifyHealthChanged();

        //fadeInOut.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            currentHealth -= 100.0f;
            if (currentHealth <= 0)
            {
                animator.SetInteger("Pattern", 6);
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            currentHealth = 0.0f;
            animator.SetInteger("Pattern", 6);
        }
    }

    // 대미지 처리
    public void TakeDamage(float damage)
    {
        if (!isInvincible)
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
    }

    // 체력 변경 알림
    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    // 보스 사망 처리
    private void Die()
    {
        //Debug.Log("Boss defeated!");
        // EndingSequencerCamera 켜기
        //EndingSequencerCamera.SetActive(false);

        animator.SetInteger("Pattern",6);
        //페이드인아웃 활성화
        //fadeInOut.SetActive(true);

        //2초뒤 페이드인아웃 비활성화
        //yield return new WaitForSeconds(2.0f);

        //페이드인아웃 비활성화
        //fadeInOut.SetActive(false);
        // 보스 사망 로직 추가 (예: 애니메이션, 제거)
        Destroy(gameObject,5f); // 보스 오브젝트 제거

    }
 

}
