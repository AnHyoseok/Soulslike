using System.Collections;
using UnityEngine;

namespace BS.Demon
{
    public class BallRise : DemonBall
    {
        #region Variables
        [SerializeField]private GameObject effgo;
        [SerializeField]private GameObject phasePattern;
        #endregion
        public override void TargetRise()
        {
            if (effgo && !phasePattern)
            {
                GameObject effcetgo = Instantiate(effgo, transform.position, Quaternion.identity);
                Destroy(effcetgo, 1.5f);
                Destroy(this.gameObject);
            }
            if (phasePattern && !effgo)
            {
                StartCoroutine(DelayRise(this.gameObject));
            }
        }
        IEnumerator DelayRise(GameObject target)
        {
            if (target != null)
            {
                GameObject effcetgo = Instantiate(phasePattern, transform.position, Quaternion.identity);
                Destroy(effcetgo, 3.5f);
                yield return new WaitForSeconds(3.5f);
                // 폭발 효과 (선택 사항)
                GameObject effectInstance = Instantiate(twoPhase.effect[0], target.transform.position, Quaternion.identity);
                Destroy(effectInstance, 0.7f);
                // 대상 제거
                Destroy(target);
            }
        }
    }
}