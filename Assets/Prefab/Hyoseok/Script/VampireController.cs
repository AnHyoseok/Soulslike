using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using BS.Player;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;
using BS.Particle;

namespace BS.vampire
{
    /// <summary>
    /// 공격상태머신
    /// </summary>
    public class VampireController : MonoBehaviour
    {
        #region Variables
        public Animator animator;
        public GameObject player;
        private int direction;  // 방향

        public float time = 20f; // 공격 대기 시간

        //패턴쿨타임
        [SerializeField] float attack1;
        [SerializeField] float attack2;
        [SerializeField] float attack3;
        [SerializeField] float attack4;
        // 공격1 
        // 두 개의 소환몹이 곡선으로 플레이어 방향으로 이동해서 타격
        public GameObject attackObjectPrefab; // 공격 관리 오브젝트
        public GameObject impactEffectPrefab; // 이펙트 프리팹 batEffect에smoke에 공격감지 넣어둠
        [SerializeField] private float attackRange;      //공격범위
        [SerializeField] private float attackCount;     //공격횟수
        public GameObject attackRangePrefab; // 공격범위 프리팹
        public Transform[] attackObjects;
        public Transform hitPoint; // 타격 지점

        //공격2
        //보스가 중앙으로 이동후 공격모션 이펙트 발동후 왼 오 아래 위 레이저 발사 
        public Transform centerTeleport;    //센터 텔레포트
        public GameObject attack2EffectPrefab; //시전 이펙트프리팹 
        public GameObject[] bloodBeams;   //왼오아래위 빔들
      

        private int nextPattern = 0;
        #endregion
        void Start()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            StartCoroutine(Attak1());
        }


        IEnumerator Attak1()
        {
            transform.LookAt(player.transform);
            yield return new WaitForSeconds(5f);
            for (int i = 0; i < attackCount; i++)
            {
                // 공격 모션 시간
                GameObject attackObject1 = Instantiate(attackObjectPrefab, attackObjects[0].position, Quaternion.identity);
                GameObject attackObject2 = Instantiate(attackObjectPrefab, attackObjects[1].position, Quaternion.identity);
                animator.SetTrigger("Attack1");

                hitPoint = player.transform;


                yield return new WaitForSeconds(0.5f);

                //공격 범위 표시
                //GameObject attackRangeIndicator = Instantiate(attackRangePrefab, hitPoint.position, Quaternion.identity);
                //VampireAttackRange rangeComponent = attackRangeIndicator.GetComponent<VampireAttackRange>();
                //rangeComponent.StartGrowing(new Vector3(1, 0.1f, 1), attackRange);
                //Destroy(attackRangeIndicator, 1f);


                Debug.Log("Attak1 start");



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
                attackObject1.transform.DOPath(path1, 0.5f, PathType.CatmullRom).SetOptions(false, AxisConstraint.Y).SetEase(Ease.InOutSine).OnUpdate(() =>
                {
                    attackObject1.transform.LookAt(player.transform);
                }).OnComplete(() =>
                {
                    // 타격 지점에 도달 시 터트리고 제거
                    GameObject impactEffect = Instantiate(impactEffectPrefab, attackObject1.transform.position, Quaternion.identity);

                    Destroy(attackObject1, 1f);
                    Destroy(impactEffect, 1.5f);
                    //Debug.Log("첫 번째 오브젝트가 타격 지점에서 제거되었습니다.");
                });

                attackObject2.transform.DOPath(path2, 0.5f, PathType.CatmullRom).SetOptions(false, AxisConstraint.Y).SetEase(Ease.InOutSine).OnUpdate(() =>
                {
                    attackObject2.transform.LookAt(player.transform);
                }).OnComplete(() =>
                {
                    // 타격 지점에 도달 시 터트리고 제거
                    GameObject impactEffect = Instantiate(impactEffectPrefab, attackObject2.transform.position, Quaternion.identity);
                    Destroy(attackObject2, 1f);
                    Destroy(impactEffect, 1.5f);
                    //Debug.Log("두 번째 오브젝트가 타격 지점에서 제거되었습니다.");
                });
                yield return new WaitForSeconds(0.1f);
            }
            NextPatternPlay();
            yield return null;
        }

        IEnumerator Attak2()
        {
            //보스 중앙이동
            transform.position = centerTeleport.position;
            //보스 스킬 연출
            GameObject skillEffectGo = Instantiate(attack2EffectPrefab, transform.position, Quaternion.identity);
            Destroy(skillEffectGo, 2f);
            //애니메이션
            animator.SetTrigger("Attack2");
            yield return new WaitForSeconds(1f);    //동작대기시간
            //이펙트

            for (int i = 0; i < bloodBeams.Length; i++)
            {
                //스킬 레이 그려야됌 일직선 빨간색 범위
     ;
                bloodBeams[i].SetActive(true);
                yield return new WaitForSeconds(2f); //레이보여주는시간
                bloodBeams[i].SetActive(false);
            }

            NextPatternPlay();
            yield return null;

            yield return new WaitForSeconds(20f);
        }

        IEnumerator Attak3()
        {
            transform.LookAt(player.transform);
            NextPatternPlay();
            yield return null;
        }

        IEnumerator Attak4()
        {
            transform.LookAt(player.transform);
            NextPatternPlay();
            yield return null;
        }



        void NextPatternPlay()
        {
            transform.LookAt(player.transform);
            nextPattern = (nextPattern % 4) + 1; // 패턴을 순환
            switch (nextPattern)
            {
                case 1:
                    StartCoroutine(Attak1());
                    Debug.Log("1번패턴실행");
                    break;
                case 2:
                    StartCoroutine(Attak2());
                    Debug.Log("2번패턴실행");
                    break;
                case 3:
                    StartCoroutine(Attak3());
                    Debug.Log("3번패턴실행");
                    break;
                case 4:
                    StartCoroutine(Attak4());
                    Debug.Log("4번패턴실행");
                    break;
            }
        }
    }
}
