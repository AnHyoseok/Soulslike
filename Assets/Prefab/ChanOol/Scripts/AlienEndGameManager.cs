using BS.Player;
using UnityEngine;
using BS.UI;
using BS.Audio;
using UnityEngine.Audio;
using BS.PlayerInput;

namespace BS.Utility
{
    public class AlienEndGameManager : MonoBehaviour
    {
        #region Variables
        private PlayerHealth playerHealth;
        private AlienHealth bossHealth;
        private DungeonClearTime dungeEndGame;
        public GameObject player;
        public GameObject boss;
        private float actorTime = 5f;
        public AudioSource audioSource;
        public AudioClip clearSound;
        public AudioClip defeatSound;
        public AudioClip titleBgm;
        private PlayerInputActions playerInputActions;

        private bool gameEnded = false;
        #endregion

        private void Start()
        {
            dungeEndGame = FindFirstObjectByType<DungeonClearTime>();
            bossHealth = FindFirstObjectByType<AlienHealth>();
            playerHealth = FindFirstObjectByType<PlayerHealth>();
            playerInputActions = player.GetComponent<PlayerInputActions>();
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
                PrepareClear();
            }
            else if (playerHealth.CurrentHealth <= 0)
            {
                PrepareDefeat();
            }
        }

        private void PrepareClear()
        {
            gameEnded = true;
            playerInputActions.enabled = false;
            audioSource.PlayOneShot(clearSound);
            audioSource.clip = clearSound;
            audioSource.Play();
            Invoke("Clear", actorTime);
        }

        private void PrepareDefeat()
        {
            gameEnded = true;
            playerInputActions.enabled = false;
            audioSource.PlayOneShot(defeatSound);
            audioSource.clip = defeatSound;
            audioSource.Play();
            Invoke("Defeat", actorTime);
        }

        private void Clear()
        {
            //Destroy(boss);

            audioSource.clip = titleBgm;
            audioSource.Play();
            dungeEndGame.CompleteDungeon();
        }

        private void Defeat()
        {
            //Destroy(boss); 
            Debug.Log("패배");
            audioSource.clip = titleBgm;
            audioSource.Play();
            dungeEndGame.DefeatDungeon();
        }
    }
}
