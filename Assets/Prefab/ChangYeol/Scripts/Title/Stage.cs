
using BS.Utility;
using Unity.VisualScripting;
using UnityEngine;

namespace BS.Title
{
    public class Stage : StageTrigger
    {
        
        public GameObject EnemyHealthbar;

        protected override void TriggerKeyDown()
        {
            stageText.text = stageName;
            if (Input.GetKeyDown(keyCode))
            {
                if (InstEnemy && !isEnemy)
                {
                    AudioUtility.CreateSFX(triggerSound, transform.position, AudioUtility.AudioGroups.Sound);
                    Enemy = Instantiate(InstEnemy, InstEnemy.transform.position, Quaternion.identity);
                    isEnemy = true;
                    EnemyHealthbar.SetActive(isEnemy);
                }
            }

        }
    }
}