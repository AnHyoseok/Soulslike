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

        private DungeonClearTime dungeon;
        #endregion
        private void Start()
        {
            dungeon = GetComponent<DungeonClearTime>();
            achievementCondition[0].gameObject.SetActive(false);
            achievementCondition[1].gameObject.SetActive(false);
            achievementCondition[2].gameObject.SetActive(false);
        }
        // Update is called once per frame
        void Update()
        {
            if(dungeon != null)
            {
                if (dungeon.isAchievement[0])
                {
                    achievementText[0].color = achievementColors[0];
                    achievementText[1].color = achievementColors[0];
                    achievementText[2].color = achievementColors[0];
                    achievementText[0].fontStyle = FontStyles.Normal;
                    achievementText[1].fontStyle = FontStyles.Normal;
                    achievementText[2].fontStyle = FontStyles.Normal;
                    achievementImages[0].sprite = achievementSprites[0];
                    achievementImages[1].sprite = achievementSprites[0];
                    achievementImages[2].sprite = achievementSprites[0];
                    achievementImages[0].color = achievementColors[0];
                    achievementImages[1].color = achievementColors[0];
                    achievementImages[2].color = achievementColors[0];
                }
                else if (dungeon.isAchievement[1])
                {
                    achievementText[0].color = achievementColors[1];
                    achievementText[1].color = achievementColors[0];
                    achievementText[2].color = achievementColors[0];
                    achievementText[0].fontStyle = FontStyles.Strikethrough;
                    achievementText[1].fontStyle = FontStyles.Normal;
                    achievementText[2].fontStyle = FontStyles.Normal;
                    achievementImages[0].sprite = achievementSprites[1];
                    achievementImages[1].sprite = achievementSprites[0];
                    achievementImages[2].sprite = achievementSprites[0];
                    achievementImages[0].color = achievementColors[1];
                    achievementImages[1].color = achievementColors[0];
                    achievementImages[2].color = achievementColors[0];
                }
                else if (dungeon.isAchievement[2])
                {
                    achievementText[0].color = achievementColors[1];
                    achievementText[1].color = achievementColors[1];
                    achievementText[2].color = achievementColors[0];
                    achievementText[0].fontStyle = FontStyles.Strikethrough;
                    achievementText[1].fontStyle = FontStyles.Strikethrough;
                    achievementText[2].fontStyle = FontStyles.Normal;
                    achievementImages[0].sprite = achievementSprites[1];
                    achievementImages[1].sprite = achievementSprites[1];
                    achievementImages[2].sprite = achievementSprites[0];
                    achievementImages[0].color = achievementColors[1];
                    achievementImages[1].color = achievementColors[1];
                    achievementImages[2].color = achievementColors[0];
                }
                else if (dungeon.isAchievement[3])
                {
                    achievementText[0].color = achievementColors[1];
                    achievementText[1].color = achievementColors[1];
                    achievementText[2].color = achievementColors[1];
                    achievementText[0].fontStyle = FontStyles.Strikethrough;
                    achievementText[1].fontStyle = FontStyles.Strikethrough;
                    achievementText[2].fontStyle = FontStyles.Strikethrough;
                    achievementImages[0].sprite = achievementSprites[1];
                    achievementImages[1].sprite = achievementSprites[1];
                    achievementImages[2].sprite = achievementSprites[1];
                    achievementImages[0].color = achievementColors[1];
                    achievementImages[1].color = achievementColors[1];
                    achievementImages[2].color = achievementColors[1];
                }
            }
        }
    }
}