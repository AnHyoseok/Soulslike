using UnityEngine;
using System.Collections.Generic;
using BS.Utility;
using UnityEngine.SceneManagement;
using System.Linq;

namespace BS.Achievement
{
    public class AchievementManager : Singleton<AchievementManager>
    {
        #region Variables
        public List<AchievementObject> achievementsGoalCondition;
        public List<AchievementData> realAchievements = new List<AchievementData>();
        public AchievementSaveDatas achievementSaveDatas;

        private AchievementSaveDataManager achievementSaveDataManager;  // 데이터 매니저 참조
        #endregion

        private void Start()
        {
            achievementSaveDataManager = GetComponent<AchievementSaveDataManager>();

            #region Linq 설명
            //DataManager.GetAchievementData() 호출: 업적 데이터를 반환하는 객체를 가져옵니다.
            //.Achievements 참조: 전체 업적 데이터를 담고 있는 객체에 접근합니다.
            //.achievements 참조: 업적 데이터를 리스트 형태로 가져옵니다.
            //.Where(...) 호출: 조건(null이 아니며, bossType과 현재 씬 이름이 동일)을 만족하는 데이터만 필터링합니다.
            //.ToList() 호출: 필터링된 결과를 새로운 리스트로 변환하여 checkAchievements에 저장합니다.
            #endregion
            realAchievements = DataManager.GetAchievementData()
                    .Achievements
                    .achievements
                    .Where(achievement => achievement != null && achievement.bossType.ToString() == SceneManager.GetActiveScene().name)
                    .ToList();
            achievementsGoalCondition = new List<AchievementObject>();

            LoadAchievementData();
        }

        private void LoadAchievementData()
        {
            // 현재 씬 이름을 기준으로 저장된 데이터를 로드
            achievementSaveDatas = achievementSaveDataManager.LoadData();

            // 로드한 데이터를 achievementSaveDatas에 추가
            //achievementSaveDatas.Add(loadData);

        }

        private void SaveAchievementData()
        {

        }
    }
}