using UnityEngine;
using BS.Achievement;
using NUnit.Framework.Interfaces;

namespace BS.Utility
{
    public class DataManager : MonoBehaviour
    {
        private static AchievementDataPool achievementDataPool;


        void Start()
        {
            //업적 데이터 가져오기
            if (achievementDataPool == null)
            {
                achievementDataPool = ScriptableObject.CreateInstance<AchievementDataPool>();
                achievementDataPool.LoadData();
            }
        }

        //업적 데이터 가져오기
        public static AchievementDataPool GetAchievementData()
        {
            if (achievementDataPool == null)
            {
                achievementDataPool = ScriptableObject.CreateInstance<AchievementDataPool>();
                achievementDataPool.LoadData();
            }
            return achievementDataPool;
        }

    }
}