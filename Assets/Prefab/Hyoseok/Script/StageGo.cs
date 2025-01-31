using BS.Player;
using BS.Utility;
using UnityEngine;

namespace BS.Title
{
    public class StageGo : StageTrigger
    {
        public GameObject StageCanvas;
        public AudioClip triggerSound;
        private bool isCanvas ;

        protected override void TriggerKeyDown()
        {
            stageText.text = stageName;
            if (Input.GetKeyDown(keyCode))
            {
                Debug.Log(isCanvas);

                isCanvas = !isCanvas;
                StageCanvas.SetActive(isCanvas);

                if (isCanvas)
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
