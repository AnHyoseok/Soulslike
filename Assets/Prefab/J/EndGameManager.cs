using BS.Player;
using UnityEngine;
using BS.UI;
using BS.Audio;
using UnityEngine.Audio;

namespace BS.Utility
{
    public class EndGameManager : MonoBehaviour
    {
        #region Variables
        private PlayerHealth playerHealth;
        private BossHealth bossHealth;
        private DungeonClearTime dungeEndGame;
        public GameObject player;
        public GameObject boss;

        public AudioSource audioSource;
        public AudioClip clearSound;
        public AudioClip defeatSound;
        public AudioClip titleBgm;

        private bool gameEnded = false; 
        #endregion

        private void Start()
        {
            //audioSource = GetComponent<AudioSource>();
            dungeEndGame = FindFirstObjectByType<DungeonClearTime>();
            bossHealth = FindFirstObjectByType<BossHealth>();
            playerHealth = FindFirstObjectByType<PlayerHealth>();
        }

        private void Update()
        {
            if (!gameEnded)
            {
                CheckGameEnd();
            }
        }

        private void CheckGameEnd()
        {
            if (bossHealth.currentHealth <= 0)
            {
                Clear();
            }
            else if (playerHealth.CurrentHealth <= 0)
            {
                Defeat();
            }
        }

        private void Clear()
        {
            
            gameEnded = true;
            Destroy(player);
            Destroy(boss);
            Debug.Log("클리어");
            audioSource.PlayOneShot(clearSound); 
            audioSource.clip = titleBgm;
            audioSource.PlayDelayed(clearSound.length);
            // 승리 연출
            dungeEndGame.CompleteDungeon();
        }

        private void Defeat()
        {
            gameEnded = true;
            Destroy(player);
            Destroy(boss);
            Debug.Log("패배");
            audioSource.PlayOneShot(defeatSound); 
            audioSource.clip = titleBgm;
            audioSource.PlayDelayed(defeatSound.length); 
            // 패배 연출 
            dungeEndGame.DefeatDungeon();
        }
    }
}
