using BS.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BS.UI
{
    public class Achievement : MonoBehaviour
    {
        #region Variables
        public TextMeshProUGUI[] achievementText;
        public TextMeshProUGUI[] achievementCondition;
        public Image[] achievementImages;
        public Sprite[] achievementSprites;
        public Color[] achievementColors;

        public DungeonClearTime dungeon;
        public PlayerHealth health;
        private float healthPercentage;
        #endregion
        private void Start()
        {
            AchievementCondition(achievementCondition[0], false);
            AchievementCondition(achievementCondition[1], false);
            AchievementCondition(achievementCondition[2], false);
        }
        // Update is called once per frame
        void Update()
        {
            if(dungeon != null)
            {
                CheckTime();
                if (health)
                {
                    CheckHealth();
                }
            }
        }
        void CheckAchievement(Color color, FontStyles styles, Sprite sprite, int index)
        {
            //텍스트 색, 폰트스타일, 스프라이트, 이미지 색
            achievementText[index].color = color;
            achievementText[index].fontStyle = styles;
            achievementImages[index].sprite = sprite;
            achievementImages[index].color = color;
        }
        void AchievementCondition(TextMeshProUGUI Condition, bool isCondition, string text = "")
        {
            //true이면 text 출력 false이면 ""출력
            text = isCondition ? $"({text})" : "";
            Condition.gameObject.SetActive(isCondition);
            Condition.text = text;
        }
        #region CheckAchievement
        void CheckHealth()
        {
            healthPercentage = (health.CurrentHealth / health.MaxHealth) * 100f;

            if (healthPercentage >= 100f)
            {
                dungeon.isHealth[0] = true;
                CheckAchievement(achievementColors[0], FontStyles.Normal, achievementSprites[0], 3);
                CheckAchievement(achievementColors[0], FontStyles.Normal, achievementSprites[0], 4);
                CheckAchievement(achievementColors[0], FontStyles.Normal, achievementSprites[0], 5);
            }
            else if (healthPercentage >= 70f)
            {
                dungeon.isHealth[1] = true;
                CheckAchievement(achievementColors[1], FontStyles.Strikethrough, achievementSprites[1], 3);
                CheckAchievement(achievementColors[0], FontStyles.Normal, achievementSprites[0], 4);
                CheckAchievement(achievementColors[0], FontStyles.Normal, achievementSprites[0], 5);
            }
            else if (healthPercentage >= 30f)
            {
                dungeon.isHealth[2] = true;
                CheckAchievement(achievementColors[1], FontStyles.Strikethrough, achievementSprites[1], 3);
                CheckAchievement(achievementColors[1], FontStyles.Strikethrough, achievementSprites[1], 4);
                CheckAchievement(achievementColors[0], FontStyles.Normal, achievementSprites[0], 5);
            }
            else
            {
                dungeon.isHealth[3] = true;
                CheckAchievement(achievementColors[1], FontStyles.Strikethrough, achievementSprites[1], 3);
                CheckAchievement(achievementColors[1], FontStyles.Strikethrough, achievementSprites[1], 4);
                CheckAchievement(achievementColors[1], FontStyles.Strikethrough, achievementSprites[1], 5);
            }
            AchievementCondition(achievementCondition[3], true, healthPercentage.ToString() + "/" + 100);
            AchievementCondition(achievementCondition[4], true, healthPercentage.ToString() + "/" + 70);
            AchievementCondition(achievementCondition[5], true, healthPercentage.ToString() + "/" + 30);
        }
        void CheckTime()
        {
            if (dungeon.isTime[0])
            {
                CheckAchievement(achievementColors[0], FontStyles.Normal, achievementSprites[0], 0);
                CheckAchievement(achievementColors[0], FontStyles.Normal, achievementSprites[0], 1);
                CheckAchievement(achievementColors[0], FontStyles.Normal, achievementSprites[0], 2);
            }
            else if (dungeon.isTime[1])
            {
                CheckAchievement(achievementColors[1], FontStyles.Strikethrough, achievementSprites[1], 0);
                CheckAchievement(achievementColors[0], FontStyles.Normal, achievementSprites[0], 1);
                CheckAchievement(achievementColors[0], FontStyles.Normal, achievementSprites[0], 2);
            }
            else if (dungeon.isTime[2])
            {
                CheckAchievement(achievementColors[1], FontStyles.Strikethrough, achievementSprites[1], 0);
                CheckAchievement(achievementColors[1], FontStyles.Strikethrough, achievementSprites[1], 1);
                CheckAchievement(achievementColors[0], FontStyles.Normal, achievementSprites[0], 2);
            }
            else if (dungeon.isTime[3])
            {
                CheckAchievement(achievementColors[1], FontStyles.Strikethrough, achievementSprites[1], 0);
                CheckAchievement(achievementColors[1], FontStyles.Strikethrough, achievementSprites[1], 1);
                CheckAchievement(achievementColors[1], FontStyles.Strikethrough, achievementSprites[1], 2);
            }
        }
        #endregion
    }
}