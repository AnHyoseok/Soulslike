using System;
using UnityEngine;

namespace BS.Achievement
{
    /// <summary>
    /// 퀘스트 목표
    /// </summary>
    [Serializable]
    public class AchievementGoal
    {
        public BossType bossType;                   //보스타입
        public AchievementType achievementType;     //업적의종류
        public float goalAmount;                    //목표
        public float currentAmount;                 //달성량

        //타임타입 변수를 활용이 가능한가?
        //public Time clearTime
        //public Time currentTime

        public bool IsCleared => (currentAmount >= goalAmount);

        public AchievementGoal(AchievementData achievementData)
        {
            bossType = achievementData.bossType;
            achievementType = achievementData.achievementType;

            switch (achievementType)
            {
                case (AchievementType.HealthBased):
                    goalAmount = achievementData.requiredHealth;
                    break;
                case AchievementType.KillCount:
                    goalAmount = achievementData.requiredKills;
                    break;
                case AchievementType.TimeBased:
                    goalAmount = achievementData.requiredTime;
                    break;
#if UNITY_EDITOR
                default:
                    Debug.LogError("AchievementType 로드 실패");
                    break;
#endif
            }
            currentAmount = 0;
        }
    }
}