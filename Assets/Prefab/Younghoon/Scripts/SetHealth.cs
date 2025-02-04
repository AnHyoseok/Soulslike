using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;


namespace BS.Enemy.Set
{
    public class SetHealth : MonoBehaviour, IDamageable
    {
        #region Variables

        [SerializeField] private float maxHealth;
        public float MaxHealth
        {
            get { return maxHealth; }
            private set { maxHealth = value; }
        }

        private float currentHealth;
        public float CurrentHealth
        {
            get { return currentHealth; }
            private set
            {
                //죽음 체크
                if (isDeath)
                    return;

                currentHealth = value;
                bosshealthBarImage.fillAmount = GetRatio();
                bossHealthText.text = $"{GetRatio() * 100:F0}%";

                if (currentHealth <= 0f)
                {
                    isDeath = true;
                    //TODO :: 죽음 구현
                    OnDie?.Invoke();
                }
            }
        }

        private bool isDeath = false;           //죽음 체크

        public float GetRatio() => CurrentHealth / MaxHealth;

        private Image bosshealthBarImage;           // 보스 체력바
        private TextMeshProUGUI bossHealthText;     // 보스 체력 텍스트

        //UnityAction
        public UnityAction OnDamaged;
        public UnityAction OnDie;

        #endregion

        private void Start()
        {
            currentHealth = MaxHealth;

            FindAndResetComponents();
        }

        #region Test용 데미지 주기 삭제 필
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                TakeDamage(10);
            }


        }
        #endregion


        public void TakeDamage(float damage)
        {
            //죽었으면 실행하지 않음
            if (isDeath) return;

            //데미지 계산
            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);

            OnDamaged?.Invoke();
        }

        private void FindAndResetComponents()
        {
            GameObject bossHealthBar = GameObject.Find("BossHealthBar");

            if (bossHealthBar != null)
            {
                // bossHealthBar의 모든 하위 오브젝트에서 컴포넌트를 가진 오브젝트들을 찾음
                // FirstOrDefault()는 첫 번째로 조건을 만족하는 요소를 반환하고, 없으면 null을 반환
                bosshealthBarImage = bossHealthBar.GetComponentsInChildren<Image>()
                    .FirstOrDefault(img => img.gameObject.name == "FillAmount");

                bossHealthText = bossHealthBar.GetComponentsInChildren<TextMeshProUGUI>()
                    .FirstOrDefault(txt => txt.gameObject.name == "HealthText");

                if (bosshealthBarImage == null)
                {
                    Debug.LogError("FillAmount 에러");
                }
                if (bossHealthText == null)
                {
                    Debug.LogError("HealthText 에러");
                }
            }
            else
            {
                Debug.LogWarning("BossHealthBar 오브젝트를 찾을 수 없습니다.");
            }



        }
    }
}