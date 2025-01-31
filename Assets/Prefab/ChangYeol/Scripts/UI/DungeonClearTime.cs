using BS.Achievement;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BS.UI
{
    public class DungeonClearTime : MonoBehaviour
    {
        #region Variables
        public TextMeshProUGUI bosstext;
        public string bossName;

        public Button[] buttons = new Button[2];
        public TextMeshProUGUI[] buttonText = new TextMeshProUGUI[2];
        public string[] buttonName = new string[4];

        //시간
        public TextMeshProUGUI timerText;
        public TextMeshProUGUI recordText;
        public TextMeshProUGUI newRecordText;
        public GameObject newRecordUI; // 신기록 UI (활성화/비활성화)

        private float elapsedTime = 0f;    // 현재 진행 시간
        private bool isDungeonActive = false; // 던전 활성화 여부
        private float bestTime = Mathf.Infinity; // 신기록 시간 (최초엔 무한대)
        [SerializeField] private float[] achievementTime;

        //시간 업적 해금
        [HideInInspector] public bool[] isTime = new bool[4];
        //체력 업적 해금
        [HideInInspector] public bool[] isHealth = new bool[4];
        #endregion

        void Update()
        {
            if (isDungeonActive)
            {
                elapsedTime += Time.deltaTime;
                UpdateTimerUI();
            }
        }
        private void UpdateTimerUI()
        {
            // 분, 초, 밀리초 계산
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            int milliseconds = Mathf.FloorToInt((elapsedTime * 1000) % 100);

            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
        // 던전 시작
        public void StartDungeon()
        {
            isDungeonActive = true;
            elapsedTime = 0f; // 경과 시간 초기화
            timerText.gameObject.SetActive(true);
            newRecordUI.SetActive(false);
        }

        // 던전 클리어
        public void CompleteDungeon()
        {
            isDungeonActive = false;
            bosstext.text = bossName;
            buttonText[0].text = buttonName[0];
            buttonText[1].text = buttonName[1];
            buttons[0].onClick.AddListener(CompleteContinue);
            buttons[1].onClick.AddListener(CompleteRetry);

            newRecordUI.SetActive(true);

            if (elapsedTime < bestTime) // 신기록 달성 여부 확인
            {
                bestTime = elapsedTime; // 신기록 갱신
                StartCoroutine(AnimateClearTime(bestTime));
                newRecordText.text = "New Record!";
            }
            else
            {
                StartCoroutine(AnimateClearTime(elapsedTime));
                newRecordText.text = "";
            }
            if (elapsedTime < achievementTime[0])
            {
                isTime[0] = true;
            }
            else if (elapsedTime < achievementTime[1])
            {
                isTime[1] = true;
            }
            else if (elapsedTime < achievementTime[2])
            {
                isTime[2] = true;
            }
            else if (elapsedTime > achievementTime[2])
            {
                isTime[3] = true;
            }

            /************************************************************************************/
            // TODO : UpdateAchievementData(TimeBased, elapsedTime) 불러오기
            /************************************************************************************/
            AchievementManager.Instance.UpdateAchievement(AchievementType.TimeBased, bestTime);

        }
        //던전 클리어 실패시
        public void DefeatDungeon()
        {
            isDungeonActive = false;
            newRecordUI.SetActive(true);
            bosstext.text = "Defeat";
            bosstext.color = Color.red;
            recordText.text = "";
            newRecordText.text = "";
            isTime[3] = true;
            buttonText[0].text = buttonName[2];
            buttonText[1].text = buttonName[3];
            buttons[0].onClick.AddListener(DefeatContinue);
            buttons[1].onClick.AddListener(DefeatRetry);
        }
        //승리시 버튼 클릭 함수
        public void CompleteContinue()
        {
            SceneManager.LoadScene("Shelter");
            //buttons[0].onClick.RemoveAllListeners();
            //buttons[1].onClick.RemoveAllListeners();
        }
        public void CompleteRetry()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;

            // 현재 씬 다시 로드
            SceneManager.LoadScene(currentSceneName);
            //buttons[0].onClick.RemoveAllListeners();
            //buttons[1].onClick.RemoveAllListeners();
        }
        //패배시 버튼 클릭 함수
        public void DefeatContinue()
        {
            SceneManager.LoadScene("Shelter");
            //buttons[0].onClick.RemoveAllListeners();
            //buttons[1].onClick.RemoveAllListeners();
        }
        public void DefeatRetry()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;

            // 현재 씬 다시 로드
            SceneManager.LoadScene(currentSceneName);
            //buttons[0].onClick.RemoveAllListeners();
            //buttons[1].onClick.RemoveAllListeners();
        }
        // 시간 포맷 변환
        private string FormatTime(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            int milliseconds = Mathf.FloorToInt((time * 1000) % 100);

            return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
        // 0부터 클리어 시간까지 숫자가 올라가는 애니메이션
        private IEnumerator AnimateClearTime(float targetTime)
        {
            float currentTime = 0f;
            float animationDuration = 4f; // 애니메이션 지속 시간
            float elapsed = 0f;

            while (elapsed < animationDuration)
            {
                elapsed += Time.deltaTime;
                currentTime = Mathf.Lerp(0f, targetTime, elapsed / animationDuration);

                recordText.text = FormatTime(currentTime); // 애니메이션 텍스트 업데이트
                yield return null;
            }

            // 애니메이션이 끝난 후 정확한 시간을 표시
            recordText.text = FormatTime(targetTime);
        }
    }
}