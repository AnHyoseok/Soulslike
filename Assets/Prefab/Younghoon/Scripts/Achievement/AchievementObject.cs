using System;
using UnityEngine;

namespace BS.Achievement
{
    /// <summary>
    /// 업적 진행 데이터 관리 클래스
    /// </summary>
    [Serializable]
    public class AchievementObject
    {
        #region Variables최초 처치

        public int number;                          //업적의 인덱스
        public AchievementType achievementType;     //업적의 타입
        public AchievementGoal achievementGoal;     //업적 목표

        public bool isUnlock = false;
        public bool isClear = false;

        #endregion

        public AchievementObject(AchievementData achievementData)
        {
            number = achievementData.number;
            achievementType = achievementData.achievementType;

            achievementGoal = new AchievementGoal(achievementData);
        }

        public void BossKill(BossType bossType)
        {
            if (achievementGoal.achievementType == AchievementType.KillCount)
            {
                if (bossType == achievementGoal.bossType)
                {
                    achievementGoal.currentAmount++;
                }
#if UNITY_EDITOR
                else
                {
                    Debug.LogError($"AchievementObject.cs / BossKill() 문제 - 변수로 받은 보스 {bossType} 지금 읽히고 있는 보스 {achievementGoal.bossType}");
                }
#endif
            }
        }

        public void SpeedRun(BossType bossType, float accumulatedTime)
        {
            if (achievementGoal.achievementType == AchievementType.TimeBased)
            {
                if (bossType == achievementGoal.bossType)
                {
                    achievementGoal.currentAmount = accumulatedTime;
                }
#if UNITY_EDITOR
                else
                {
                    Debug.LogError($"AchievementObject.cs / EnemyKill() 문제 - 변수로 받은 보스 {bossType} 지금 읽히고 있는 보스 {achievementGoal.bossType}");
                }
#endif
            }
        }

        public void DamageCheck(BossType bossType, float damage)
        {
            if (achievementGoal.achievementType == AchievementType.TimeBased)
            {
                if (bossType == achievementGoal.bossType)
                {
                    achievementGoal.currentAmount = damage;
                }
#if UNITY_EDITOR
                else
                {
                    Debug.LogError($"AchievementObject.cs / EnemyKill() 문제 - 변수로 받은 보스 {bossType} 지금 읽히고 있는 보스 {achievementGoal.bossType}");
                }
#endif
            }
        }

    }
}