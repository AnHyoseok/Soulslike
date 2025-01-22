
using Unity.VisualScripting;
using UnityEngine;

namespace BS.Title
{
    public class Stage : StageTrigger
    {

     
        protected override void TriggerKeyDown()
        {
            stageText.text = stageName;
            if (Input.GetKeyDown(keyCode))
            {
              
                //Debug.Log("true");
              
                 if (InstEnemy && !isEnemy)
                {
                    Enemy = Instantiate(InstEnemy, InstEnemy.transform.position,Quaternion.identity);
                    isEnemy = true;
                }
            }
        
        }
    }
}