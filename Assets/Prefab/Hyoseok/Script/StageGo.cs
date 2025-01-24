using BS.Utility;
using UnityEngine;

namespace BS.Title
{
    public class StageGo : StageTrigger
    {
        public GameObject StageCanvas;
        public AudioClip triggerSound;

        protected override void TriggerKeyDown()
        {
            stageText.text = stageName;
            if (Input.GetKeyDown(keyCode))
            {
                if (StageCanvas)
                {
                    StageCanvas.SetActive(!StageCanvas.activeSelf);
                    if(StageCanvas.activeSelf)
                    {
                        AudioUtility.CreateSFX(triggerSound, transform.position, AudioUtility.AudioGroups.Sound);
                        inputActions.UnInputActions();
                    }
                    else
                    {
                        inputActions.OnInputActions();
                    }
                }
            }

        }
    }
}