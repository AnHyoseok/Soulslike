using BS.Player;
using UnityEngine;
using BS.UI;
using BS.Audio;
using UnityEngine.Audio;
using BS.PlayerInput;
using BS.Demon;
using Unity.Cinemachine;
using BS.Enemy.Set;
using BS.Managers;
using System;
using System.Collections;

namespace BS.Utility
{
    public class SetEndGameManager : MonoBehaviour
    {
        #region Variables
        private PlayerHealth playerHealth;
        private SetHealth bossHealth;

        private SetDungeonClearTime dungeonEndGame;

        public AudioSource audioSource;

        public AudioClip clearBGM;
        public AudioClip defeatBGM;

        private Camera mainCamera;

        private bool isEnding;
        [SerializeField] private float endingFieldOfView = 30f;
        [SerializeField] private float zoomSpeed = 2f;

        [SerializeField] private float showAchievementUITimer = 5f;
        #endregion

        private void Start()
        {
            isEnding = false;

            mainCamera = Camera.main;

            dungeonEndGame = FindFirstObjectByType<SetDungeonClearTime>();
            bossHealth = FindFirstObjectByType<SetHealth>();
            playerHealth = FindFirstObjectByType<PlayerHealth>();

            bossHealth.OnDie += EndingProduction;
            playerHealth.OnDie += EndingProduction;
        }

        private void Update()
        {
            if (isEnding)
            {
                EndingCameraProduction();
            }
        }

        private void EndingCameraProduction()
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, endingFieldOfView, Time.unscaledDeltaTime * zoomSpeed);
        }

        private void EndingProduction()
        {
            dungeonEndGame.StopTimer();
            Time.timeScale = 0.3f;
            mainCamera.GetComponent<CameraManager>().enabled = false;
            if (playerHealth.GetRatio() > bossHealth.GetRatio())
            {
                mainCamera.transform.LookAt(bossHealth.gameObject.transform);
                audioSource.clip = clearBGM;
            }
            else
            {
                mainCamera.transform.LookAt(playerHealth.gameObject.transform);
                audioSource.clip = defeatBGM;
            }
            audioSource.Play();
            isEnding = true;
            StartCoroutine(ShowAchivementManager());
        }

        private IEnumerator ShowAchivementManager()
        {
            yield return new WaitForSecondsRealtime(showAchievementUITimer);
            Time.timeScale = 1f;
            dungeonEndGame.CompleteDungeon();
        }
    }
}
