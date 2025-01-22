using BS.Audio;
using BS.Utility;
using System.Collections;
using UnityEngine;

namespace BS.Demon
{
    public class BallRise : DemonBall
    {
        #region Variables
        [SerializeField]private GameObject effgo;
        [SerializeField]private GameObject phasePattern;
        [SerializeField]private DemonAudioManager audioManager;
        #endregion
        public override void TargetRise()
        {
            if (effgo && !phasePattern)
            {
                StartCoroutine(DelayRise(this.gameObject));
            }
            if (phasePattern && !effgo)
            {
                StartCoroutine(PhaseDelayRise(this.gameObject));
            }
        }
        IEnumerator PhaseDelayRise(GameObject target)
        {
            if (target != null)
            {
                GameObject effcetgo = Instantiate(phasePattern, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.5f);
                AudioUtility.CreateSFX(audioManager.sounds[4].audioClip, transform.position,audioManager.sounds[4].group);
                Destroy(effcetgo, 3.5f);
                yield return new WaitForSeconds(3.5f);
                // 폭발 효과 (선택 사항)
                GameObject effectInstance = Instantiate(twoPhase.effect[0], target.transform.position, Quaternion.identity);
                // 대상 제거
                if(!effcetgo)
                {
                    Destroy(effectInstance, 0.7f);
                    AudioUtility.CreateSFX(audioManager.sounds[3].audioClip, transform.position, audioManager.sounds[3].group);
                    Destroy(target);
                }
            }
        }
        IEnumerator DelayRise(GameObject target)
        {
            if(target != null)
            {
                GameObject effcetgo = Instantiate(effgo, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
                AudioUtility.CreateSFX(audioManager.sounds[0].audioClip, target.transform.position, audioManager.sounds[0].group);
                Destroy(effcetgo, 1.5f);
                Destroy(target);
            }
        }
    }
}