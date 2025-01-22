
using Unity.VisualScripting;
using UnityEngine;

namespace BS.Title
{
    public class StageGo : StageTrigger
    {
        public GameObject StageCanvas;

        protected override void TriggerKeyDown()
        {
            stageText.text = stageName;
            if (Input.GetKeyDown(keyCode))
            {

            
                if (StageCanvas)
                {
                    StageCanvas.SetActive(!StageCanvas.activeSelf);
                }
            }

        }
    }
}