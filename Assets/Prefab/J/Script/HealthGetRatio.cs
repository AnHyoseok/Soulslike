using BS.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BS.UI
{
    public class HealthGetRatio : MonoBehaviour
    {
        #region Variables
        public BossHealth bossHealth;
        public PlayerHealth playerHealth;
        public Image bosshealthBarImage;
        public Image playerhealthBarImage;
        public TextMeshProUGUI bossHealthText; // 보스 체력 텍스트
        public TextMeshProUGUI playerHealthText; // 플레이어 체력 텍스트
        public TextMeshProUGUI potionText;  // 포션 갯수 텍스트
        #endregion



        private void Update()
        {
            // 컴포넌트가 할당되었는지 확인 후 업데이트
            if (bossHealth != null && playerHealth != null)
            {
                float bossHealthRatio = bossHealth.currentHealth / bossHealth.maxHealth;
                float playerHealthRatio = playerHealth.CurrentHealth / playerHealth.MaxHealth;

                bosshealthBarImage.fillAmount = bossHealthRatio;
                playerhealthBarImage.fillAmount = playerHealthRatio;

                bossHealthText.text = $"{bossHealthRatio * 100:F0}%"; // 보스 체력 퍼센트 
                playerHealthText.text = $"{playerHealthRatio * 100}%"; // 플레이어 체력 퍼센트
            }
        }
    }
}
