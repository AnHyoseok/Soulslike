
using BS.Utility;
using Unity.VisualScripting;
using UnityEngine;

namespace BS.Title
{
    public class Stage : StageTrigger
    {
        public AudioClip triggerSound;

        protected override void TriggerKeyDown()
        {
            stageText.text = stageName;
            if (Input.GetKeyDown(keyCode))
            {

                //Debug.Log("true");

                if (InstEnemy && !isEnemy)
                {

                    AudioUtility.CreateSFX(triggerSound, transform.position, AudioUtility.AudioGroups.Sound);
                    Enemy = Instantiate(InstEnemy, InstEnemy.transform.position, Quaternion.identity);
                    isEnemy = true;
                }
            }

        }
    }
}