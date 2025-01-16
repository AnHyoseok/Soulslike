using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.Arm;

namespace BS.Demon
{
    [System.Serializable]
    public class PatternPoins
    {
        public Transform[] patternTwoPoints;
    }
    public class TwoPhasePattern : MonoBehaviour
    {
        #region Variables
        private DemonPattern pattern;

        //패턴 4
        public List<PatternPoins> patternTwoPoints;
        public float spawnTime = 1f;
        public GameObject[] effect;
        public GameObject effectdot;

        public Transform balltransform;
        #endregion
        private void Start()
        {
            pattern = GetComponent<DemonPattern>();
        }
        //2페이지 패턴 1
        #region 2Phase Pattern 1
        public void SpawnAndExplodeInOrder()
        {
            if (patternTwoPoints.Count == 0 || !pattern.ball[1])
            {
                Debug.LogWarning("Spawn points or object to spawn not set!");
                return;
            }
            StartCoroutine(SpawnAndExplodeRoutine());
            pattern.demon.lastPesosTime[0] = Time.time;
        }

        private IEnumerator SpawnAndExplodeRoutine()
        {
            foreach (PatternPoins points in patternTwoPoints)
            {
                foreach (Transform transform in points.patternTwoPoints)
                {
                    Debug.Log("sss");
                    GameObject Range = Instantiate(pattern.attackRangePrefab[0], transform.position + new Vector3(0, 0.2f, 0), Quaternion.identity);
                    Range.GetComponent<DemonAttackRange>().StartGrowing(pattern.attackRangeScale[0], pattern.rangeSize[0]);
                    Destroy(Range, 1f);
                    Debug.Log("ddssds");

                    // 지정된 위치에 오브젝트 생성
                    GameObject spawnedBall = Instantiate(pattern.ball[1].gameObject, transform.position, Quaternion.identity);
                    spawnedBall.GetComponent<BallRise>().StartRise();
                }
                yield return new WaitForSeconds(spawnTime);
            }
        }
        #endregion
        #region 2Phase Pattern 2
        public void AttackBallDot()
        {
            GameObject attackball = Instantiate(pattern.ballInstantiate, pattern.ballTranfrom.position, Quaternion.identity);
            GameObject effgo = Instantiate(effect[1], attackball.transform.position, Quaternion.identity);
            Destroy(attackball, 1f);
            Destroy(effgo, 1f);
            Vector3 Explosionpos = new Vector3(effgo.transform.position.x, attackball.transform.position.y + -1.9f, effgo.transform.position.z);
            GameObject Explosion = Instantiate(effect[2], Explosionpos, Quaternion.identity);
            GameObject dot = Instantiate(effectdot, Explosionpos, effectdot.transform.rotation);
            Destroy(Explosion, 1f);
            StartCoroutine(EffectDot(dot));
            pattern.demon.lastPesosTime[1] = Time.time;
        }
        public void DelayRange()
        {
            StartCoroutine(AttackRangeBall());
        }
        IEnumerator AttackRangeBall()
        {
            GameObject Range = Instantiate(pattern.attackRangePrefab[0], balltransform.transform.position, Quaternion.identity);
            Range.GetComponent<DemonAttackRange>().StartGrowing(pattern.attackRangeScale[1], pattern.rangeSize[1]);
            yield return new WaitForSeconds(1);
            Destroy(Range);
        }
        IEnumerator EffectDot(GameObject effdot)
        {
            if (effdot != null)
            {
                yield return new WaitForSeconds(13);
                Destroy(effdot);
            }
        }
        #endregion
    }
}