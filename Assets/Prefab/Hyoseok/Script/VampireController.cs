using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using BS.Player;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

namespace BS.vampire
{
    /// <summary>
    /// 돌진, 슛, 슛2, 소환
    /// </summary>
    public class VampireController : MonoBehaviour
    {
        #region Variables
        public GameObject player;
        private int direction;  // 방향

        public float time = 20f; // 공격 대기 시간

        // 공격1
        // 두 개의 소환몹이 곡선으로 플레이어 방향으로 이동해서 타격
        public GameObject attackObjectPrefab; // 공격 관리 오브젝트
        public GameObject impactEffectPrefab; // 이펙트 프리팹
        public Transform[] attackObjects;
        public Transform hitPoint; // 타격 지점

        private int nextPattern = 0;
        #endregion

        void lookPlayer()
        {
            direction = (player.GetComponent<Transform>().position.x < transform.position.x ? -1 : 1);
            float scale = transform.localScale.z;
            transform.localScale = new Vector3(direction * -1 * scale, scale, scale);
        }

        IEnumerator Attak1()
        {
            yield return new WaitForSeconds(5f);
            // 공격 모션 시간
            GameObject attackObject1 = Instantiate(attackObjectPrefab, attackObjects[0].position, Quaternion.identity);
            GameObject attackObject2 = Instantiate(attackObjectPrefab, attackObjects[1].position, Quaternion.identity);
            yield return new WaitForSeconds(1f);

            Debug.Log("Attak1 start");
            lookPlayer();
            hitPoint = player.transform;

            // 이동 경로 (y 값 고정, x 및 z 값 변경)
            Vector3[] path1 = {
                attackObject1.transform.position,
                new Vector3((attackObject1.transform.position.x + hitPoint.position.x) / 2, attackObject1.transform.position.y, hitPoint.position.z + 2),
                hitPoint.position
            };

            Vector3[] path2 = {
                attackObject2.transform.position,
                new Vector3((attackObject2.transform.position.x + hitPoint.position.x) / 2, attackObject2.transform.position.y, hitPoint.position.z - 2),
                hitPoint.position
            };

            // 곡선 이동 및 플레이어 바라보게 설정
            attackObject1.transform.DOPath(path1, 1f, PathType.CatmullRom).SetOptions(false, AxisConstraint.Y).SetEase(Ease.InOutSine).OnUpdate(() =>
            {
                attackObject1.transform.LookAt(player.transform); 
            }).OnComplete(() => {
                // 타격 지점에 도달 시 터트리고 제거
                GameObject impactEffect = Instantiate(impactEffectPrefab, attackObject1.transform.position, Quaternion.identity);
       
                Destroy(attackObject1);
                Destroy(impactEffect, 3f);
                Debug.Log("첫 번째 오브젝트가 타격 지점에서 제거되었습니다.");
            });

            attackObject2.transform.DOPath(path2, 1f, PathType.CatmullRom).SetOptions(false, AxisConstraint.Y).SetEase(Ease.InOutSine).OnUpdate(() =>
            {
                attackObject2.transform.LookAt(player.transform); 
            }).OnComplete(() => {
                // 타격 지점에 도달 시 터트리고 제거
                GameObject impactEffect = Instantiate(impactEffectPrefab, attackObject2.transform.position, Quaternion.identity);
                Destroy(attackObject2);
                Destroy(impactEffect, 3f);
                Debug.Log("두 번째 오브젝트가 타격 지점에서 제거되었습니다.");
            });

            NextPatternPlay();
            yield return null;
        }

        IEnumerator Attak2()
        {
            NextPatternPlay();
            yield return null;
        }

        IEnumerator Attak3()
        {
            NextPatternPlay();
            yield return null;
        }

        IEnumerator Attak4()
        {
            NextPatternPlay();
            yield return null;
        }

        void Start()
        {
            StartCoroutine(Attak1());
        }

        void NextPatternPlay()
        {
            nextPattern = (nextPattern % 4) + 1; // 패턴을 순환
            switch (nextPattern)
            {
                case 1:
                    StartCoroutine(Attak1());
                    break;
                case 2:
                    StartCoroutine(Attak2());
                    break;
                case 3:
                    StartCoroutine(Attak3());
                    break;
                case 4:
                    StartCoroutine(Attak4());
                    break;
            }
        }
    }
}
