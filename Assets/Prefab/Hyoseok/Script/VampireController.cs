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
        public float testAttackNumber;

        private int direction;  // 방향

        public float time = 20f; // 공격 대기 시간

        //패턴쿨타임
        [SerializeField] float attack1;
        [SerializeField] float attack2;
        [SerializeField] float attack3;
        [SerializeField] float attack4;
        [Header("Attack1")]
        // 공격1 
        // 두 개의 소환몹이 곡선으로 플레이어 방향으로 이동해서 타격
        public GameObject attackObjectPrefab; // 공격 관리 오브젝트
        public GameObject impactEffectPrefab; // 이펙트 프리팹 batEffect에smoke에 공격감지 넣어둠
        [SerializeField] private float attackRange;      //공격범위
        [SerializeField] private float attackCount;     //공격횟수
        public GameObject attackRangePrefab; // 공격범위 프리팹
        public Transform[] attackObjects;
        public Transform hitPoint; // 타격 지점
        [Header("Attack2")]
        //공격2
        //보스가 중앙으로 이동후 공격모션 이펙트 발동후 왼 오 아래 위 레이저 발사 
        public Transform centerTeleport;    //센터 텔레포트
        public GameObject attack2EffectPrefab; //시전 이펙트프리팹 
        public GameObject[] bloodBeams;   //왼오아래위 빔들
        public GameObject[] attak3Ranges;
        [Header("Attack3")]
        //공격3
        //보스가 플레이어를 바라보며 부채꼴 박쥐웨이브 날리기
        public GameObject Attack3BatPrefab; //부딫히면 플레이어에게 데미지주는 박쥐
        public float waveCount = 3f;     //공격 웨이브 카운트
        public GameObject attack3EffectPrefab;  //부딫혔을때 파티클
        public Transform[] batTransforms;

        [Header("Attack4")]
        //공격4
        //보스 주변에 박쥐들(로테이션들이) 랜덤 1~2 방향으로 산개후 플레이어방향으로 레이져 발사
        public GameObject[] summonObject;    //소환위치
        public GameObject attak4Ranges;
        public float moveRadius = 2f; // 이동 반경
        public GameObject attack4EffectPrefab;  //레이저 이펙트
        private Vector3[] originalPositions; // 원래위치
        [SerializeField] float attack4count = 3f;  //반복횟수
        private int nextPattern = 0;
        #endregion
        void Start()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            StartCoroutine(Attack1());



        }


        IEnumerator Attack1()
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

        IEnumerator Attack2()
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

            for (int i = 0; i < attak3Ranges.Length; i++)
            {
                //스킬 레이 그려야됌 일직선 빨간색 범위
                attak3Ranges[i].SetActive(true);
                yield return new WaitForSeconds(1f);
                attak3Ranges[i].SetActive(false);
            }

            for (int i = 0; i < bloodBeams.Length; i++)
            {
                //레이보여주는시간
                bloodBeams[i].SetActive(true);
                yield return new WaitForSeconds(1f);
                bloodBeams[i].SetActive(false);
            }

            NextPatternPlay();
            yield return null;

            yield return new WaitForSeconds(10f);
        }

        IEnumerator Attack3()
        {
            yield return new WaitForSeconds(7f);
            transform.LookAt(player.transform);
            //웨이브
            for (int i = 0; i < waveCount; i++)
            {
                foreach (Transform batTransform in batTransforms)
                {
                    GameObject bat = Instantiate(Attack3BatPrefab, batTransform.position, batTransform.rotation);
                    Rigidbody rb = bat.GetComponent<Rigidbody>();


                    Vector3 directionToPlayer = (player.transform.position - batTransform.position).normalized;
                    directionToPlayer.y = 0;
                    //directionToPlayer.z = 0;
                    Attack3Bat attack3Bat = bat.GetComponent<Attack3Bat>();
                    attack3Bat.Initialize(directionToPlayer, 20f);

                    Destroy(bat, 7f);

                    //충돌시 이펙트 



                }
                yield return new WaitForSeconds(0.3f);
            }
            NextPatternPlay();
            yield return null;
        }

        IEnumerator Attack4()
        {
            yield return new WaitForSeconds(7f);

            for (int j = 0; j < attack4count; j++)
            {
                

                // 공격4용 위치 저장
                originalPositions = new Vector3[summonObject.Length];
                for (int i = 0; i < summonObject.Length; i++)
                {
                    originalPositions[i] = summonObject[i].transform.position;
                }

                // 위치 이동
                float elapsedTime = 0f;
                float moveDuration = 0.2f; // 이동에 걸릴 시간

                Vector3[] targetPositions = new Vector3[summonObject.Length];
                for (int i = 0; i < summonObject.Length; i++)
                {
                    targetPositions[i] = originalPositions[i] + new Vector3(Random.Range(-moveRadius, moveRadius), 0, Random.Range(-moveRadius, moveRadius));
                }

                while (elapsedTime < moveDuration)
                {
                    for (int i = 0; i < summonObject.Length; i++)
                    {
                        if (summonObject[i] != null)
                        {
                            Vector3 newPosition = Vector3.Lerp(originalPositions[i], targetPositions[i], elapsedTime / moveDuration);
                            newPosition.y = originalPositions[i].y; // y값 고정
                            summonObject[i].transform.position = newPosition;

                            // 플레이어를 바라보도록 회전한 후, 랜덤한 값을 더하기
                            summonObject[i].transform.LookAt(player.transform);
                            Vector3 eulerAngles = summonObject[i].transform.rotation.eulerAngles;
                            eulerAngles.y += Random.Range(-10f, 10f);
                            //eulerAngles.z += Random.Range(-10f, 10f);
                            summonObject[i].transform.rotation = Quaternion.Euler(eulerAngles);
                        }
                    }
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }



                for (int i = 0; i < summonObject.Length; i++)
                {
                    if (summonObject[i] != null)
                    {
                        summonObject[i].transform.position = targetPositions[i];
                    }
                }

                //공격범위
                for( int i = 0; i< summonObject.Length; i++)
                {
                    GameObject attakRange = Instantiate(attak4Ranges, summonObject[i].transform.position, summonObject[i].transform.rotation);
                    attakRange.transform.parent = summonObject[i].transform;
                    Destroy(attakRange, 0.5f);
                }

                yield return new WaitForSeconds(0.5f);
                // 레이저 발사
                for (int i = 0; i < summonObject.Length; i++)
                {
                    GameObject attackEffect = Instantiate(attack4EffectPrefab, summonObject[i].transform.position, summonObject[i].transform.rotation);
                    attackEffect.transform.parent = summonObject[i].transform;
                    attackEffect.transform.rotation *= Quaternion.Euler(90f, 0f, 0f);
                    Destroy(attackEffect, 0.5f);
                }


                yield return new WaitForSeconds(1f);

                // 다시 원래 위치로
                elapsedTime = 0f;
                while (elapsedTime < moveDuration)
                {
                    for (int i = 0; i < summonObject.Length; i++)
                    {
                        if (summonObject[i] != null)
                        {
                            summonObject[i].transform.position = Vector3.Lerp(targetPositions[i], originalPositions[i], elapsedTime / moveDuration);
                        }
                    }
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                for (int i = 0; i < summonObject.Length; i++)
                {
                    if (summonObject[i] != null)
                    {
                        summonObject[i].transform.position = originalPositions[i];
                    }
                }
            }
            yield return new WaitForSeconds(3f);
            NextPatternPlay();
            yield return null;
        }

        void StraightMove()
        {

        }


        void NextPatternPlay()
        {
            transform.LookAt(player.transform);
            nextPattern = (nextPattern % 4) + 1; // 패턴을 순환
            switch (nextPattern)
            {
                case 1:
                    StartCoroutine(Attack1());
                    Debug.Log("1번패턴실행");
                    break;
                case 2:
                    StartCoroutine(Attack2());
                    Debug.Log("2번패턴실행");
                    break;
                case 3:
                    StartCoroutine(Attack3());
                    Debug.Log("3번패턴실행");
                    break;
                case 4:
                    StartCoroutine(Attack4());
                    Debug.Log("4번패턴실행");
                    break;
            }
        }
    }
}
