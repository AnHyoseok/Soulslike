using BS.Player;
using UnityEngine;
using BS.UI;
using BS.Audio;
using UnityEngine.Audio;
using BS.PlayerInput;
using BS.vampire;

namespace BS.Utility
{
    public class VampireEndGameManager : MonoBehaviour
    {
        #region Variables
        private PlayerHealth playerHealth;
        private VampireHealth bossHealth;
        private DungeonClearTime dungeEndGame;
        public GameObject player;
        public GameObject boss;
        public GameObject VampireDummy;
        [SerializeField] private float actorTime=3f;
        public AudioSource audioSource;
        public AudioClip clearSound;
        public AudioClip defeatSound;
        public Animator animator;
   
        private PlayerInputActions playerInputActions;

        private bool gameEnded = false;
        #endregion

        private void Start()
        {
            animator= VampireDummy.GetComponent<Animator>();
            dungeEndGame = FindFirstObjectByType<DungeonClearTime>();
            bossHealth = FindFirstObjectByType<VampireHealth>();
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
            dungeEndGame.StopTimer();
            gameEnded = true;
            playerInputActions.enabled = false;
            VampireDummy.SetActive(true);
            animator.SetTrigger("Win");
            boss.GetComponent<VampireController>().enabled = false;
            boss.GetComponent<PattonSummon>().enabled = false;
            audioSource.clip = clearSound;
            audioSource.Play();
            Invoke("Clear", actorTime); 
        }

        private void PrepareDefeat()
        {
            gameEnded = true;
            playerInputActions.enabled = false;
            VampireDummy.SetActive(true);
            boss.GetComponent<VampireController>().enabled = false;
            boss.GetComponent<PattonSummon>().enabled = false;
            animator.SetTrigger("Defeat");
            audioSource.clip = defeatSound;
            audioSource.Play();
            Invoke("Defeat", actorTime); 
        }

        private void Clear()
        {
          
            Destroy(boss);
         
        
            dungeEndGame.CompleteDungeon();
        }

        private void Defeat()
        {
            Destroy(boss); 
            Debug.Log("패배");
        
            dungeEndGame.DefeatDungeon();
        }
    }
}
