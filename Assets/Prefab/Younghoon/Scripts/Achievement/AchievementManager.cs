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
        public List<AchievementObject> achievementsGoalCondition = new List<AchievementObject>();
        public List<AchievementData> realAchievements = new List<AchievementData>();
        public AchievementSaveDatas achievementSaveDatas;

        private AchievementSaveDataManager achievementSaveDataManager;  // 데이터 매니저 참조

        #region Test용 변수(추후 삭제 필)
        public float time = 0.0f;
        #endregion

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

            LoadAchievementData();

            for (int i = 0; i < realAchievements.Count; i++)
            {
                achievementsGoalCondition.Add(new AchievementObject(realAchievements[i]));
                achievementsGoalCondition[i].isUnlock = achievementSaveDatas.achievementSaveData[i].isUnlock;
                achievementsGoalCondition[i].isClear = achievementSaveDatas.achievementSaveData[i].isClear;
                achievementsGoalCondition[i].achievementGoal.currentAmount = achievementSaveDatas.achievementSaveData[i].currentAmount;
                achievementsGoalCondition[i].achievementGoal.nextStep = realAchievements[i].next > 0;
            }
        }

        private void Update()
        {
            time += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.L))
            {
                UpdateAchievement(AchievementType.KillCount, 1);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                UpdateAchievement(AchievementType.TimeBased, time);
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                SaveAchievementData();
            }
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
            for (int i = 0; i < achievementSaveDatas.achievementSaveData.Count; i++)
            {
                achievementSaveDatas.achievementSaveData[i].isUnlock = achievementsGoalCondition[i].isUnlock;
                achievementSaveDatas.achievementSaveData[i].isClear = achievementsGoalCondition[i].isClear;
                achievementSaveDatas.achievementSaveData[i].currentAmount = achievementsGoalCondition[i].achievementGoal.currentAmount;
            }
            achievementSaveDataManager.SaveData(achievementSaveDatas);
        }

        public void UpdateAchievement(AchievementType type, float amount)
        {
            //var achievementsGoal = achievementsGoalCondition.Find(d=>d.achievementType == type);
            List<AchievementObject> list = new List<AchievementObject>();
            foreach (var achievementsGoal in achievementsGoalCondition)
            {
                if (type == achievementsGoal.achievementType)
                {
                    list.Add(achievementsGoal);
                }
            }
            Debug.Log(list.Count);
            switch (type)
            {
                case AchievementType.HealthBased:
                    //TODO
                    break;
                case AchievementType.KillCount:
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].BossKill();
                        if (list[i].achievementGoal.IsCleared(type))
                        {
                            list[i].isClear = true;
                            Debug.Log($"i = {i}");
                            if (list.Count > i + 1)
                            {
                                list[i + 1].isUnlock = true;
                            }
                        }
                    }
                    break;
                case AchievementType.TimeBased:
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].ClearTime(amount);
                        if (list[i].achievementGoal.IsCleared(type))
                        {
                            list[i].isClear = true;
                            if (list.Count > i + 1)
                            {
                                list[i + 1].isUnlock = true;
                            }
                        }
                    }
                    break;
                default:
                    Debug.LogWarning($"{type}이 잘못되었습니다.");
                    break;

            }



        }
    }
}