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
        public GameObject player;   //바닥
        public GameObject playerBody;   //플레이어몸체 
        public float testAttackNumber;

        private int direction;  // 방향

        public float time = 20f; // 공격 대기 시간

        //패턴쿨타임
        [SerializeField] float attack1;
        [SerializeField] float attack2;
        [SerializeField] float attack3;
        [SerializeField] float attack4;

        [Header("Teleport")]
        public GameObject pingpongShot;
        public GameObject CircleShot;
        public GameObject teleportEffect;
        public Transform[] teleports; //순간이동 위치 0~3 랜덤 4는 중앙
        public float teleportTime = 20; //순간이동 쿨타임
        private int previousIndex = -1; //이전 위치값

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
        //public GameObject teleportEffect;
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

        [Header("Attack5")]
        //공격5
        //보스가 위 오른쪽에 레이저이동박쥐 소환 
        public Transform[] attack5Lotations;
        public GameObject attack5BatPrefab;
        public GameObject attack5SummonEffect;
        private bool isAttack5BatSummon = false;  //배트소환 여부

        [Header("Attack6")]
        //공격 6 즉사기
        //보스가 공중날기 이후 기모아서 메테오 시전
        // 공중날기하는 중에 위험구역과 안전지대 생성
        // 안전 지대에 있을 시 플레이어 무적 , 그 밖에 즉사 
        public GameObject attack6Range;
        public GameObject[] safeRanges;
        public GameObject attack6BossEffect;
        public GameObject meteorPrefab;



        private int nextPattern = 0;
        #endregion
        void Start()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            StartCoroutine(Attack6());
            //NextPatternPlay();


        }

        //private void Update()
        //{
        //    transform.LookAt(player.transform);
        //}
        IEnumerator RandomTeleport()
        {
          
            yield return new WaitForSeconds(time);
            // 애니메이션 연출 3초 후에 이동
            animator.SetTrigger("Teleport");
            GameObject potalEffect = Instantiate(teleportEffect, transform.position, Quaternion.identity);
            potalEffect.transform.parent = transform;
            Destroy(potalEffect, 3.3f);
            yield return new WaitForSeconds(2.5f);
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, 3);
            }
            while (randomIndex == previousIndex); // 같은 값 연속 방지
                                                  // 보스 위치를 랜덤 이동
            transform.position = teleports[randomIndex].position;
            previousIndex = randomIndex;

            // 플레이어 바라보며 걷기
            transform.LookAt(player.transform.position);
            // animator.SetTrigger("Walk");

            float walkDuration = 5f;
            float elapsedTime = 0f;

            Vector3 startPos = transform.position;
            Vector3 endPos = transform.position + new Vector3(transform.forward.x, 0, transform.forward.z) * 5f;

            // 걷는 동안 탄막 발사
            // 생성으로 교체
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            GameObject tan = Instantiate(pingpongShot, spawnPosition, pingpongShot.transform.rotation);
            Destroy(tan, 10f);

            while (elapsedTime < walkDuration)
            {
                transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / walkDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        //배트자폭
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

                Debug.Log("Attack1 start");

                // 이동 경로 (y 값을 0으로 설정)
                Vector3[] path1 = {
            new Vector3(attackObject1.transform.position.x, 0, attackObject1.transform.position.z),
            new Vector3((attackObject1.transform.position.x + hitPoint.position.x) / 2, 0, hitPoint.position.z + 2),
            new Vector3(hitPoint.position.x, 0, hitPoint.position.z)
        };

                Vector3[] path2 = {
            new Vector3(attackObject2.transform.position.x, 0, attackObject2.transform.position.z),
            new Vector3((attackObject2.transform.position.x + hitPoint.position.x) / 2, 0, hitPoint.position.z - 2),
            new Vector3(hitPoint.position.x, 0, hitPoint.position.z)
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
                });

                yield return new WaitForSeconds(0.1f);
            }
            NextPatternPlay();
            yield return null;
        }

        //상하좌우 레이져 패턴
        IEnumerator Attack2()
        {
            //보스 중앙이동
            //animator.SetTrigger("Teleport");
            GameObject potalEffect = Instantiate(teleportEffect, transform.position, Quaternion.identity);
            potalEffect.transform.parent = transform;
            Destroy(potalEffect, 3.3f);
            yield return new WaitForSeconds(2.5f);
            transform.position = centerTeleport.position;
            transform.LookAt(player.transform);
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

        }

        //배트날리기
        IEnumerator Attack3()
        {
            yield return new WaitForSeconds(5f);
            transform.LookAt(player.transform);
            animator.SetTrigger("Attack1");
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
        //소환된 배트가 플레이어타격 레이져발사
        IEnumerator Attack4()
        {
            yield return new WaitForSeconds(7f);
            transform.LookAt(player.transform);
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
                animator.SetTrigger("Attack1");
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
                            summonObject[i].transform.LookAt(playerBody.transform);

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
                for (int i = 0; i < summonObject.Length; i++)
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
        //레이저 배트소환
        IEnumerator Attack5()
        {
            if (isAttack5BatSummon)
            {
                NextPatternPlay();
                yield break;
            }
            transform.LookAt(player.transform);
            isAttack5BatSummon = true;
            animator.SetTrigger("Attack1");
            GameObject summonEffect = Instantiate(attack5SummonEffect, transform.position, Quaternion.identity);
            summonEffect.transform.parent = transform;
            Destroy(summonEffect, 3f);
            yield return new WaitForSeconds(3f);
            // 공격 모션 시간
            GameObject attackObject1 = Instantiate(attack5BatPrefab, attack5Lotations[0].position, attack5Lotations[0].rotation);
            GameObject attackObject2 = Instantiate(attack5BatPrefab, attack5Lotations[1].position, attack5Lotations[1].rotation);


            // 첫 번째 오브젝트 (z축으로 이동)
            Attack5BatMove firstObject = attackObject1.GetComponent<Attack5BatMove>();
            firstObject.moveInZAxis = true;

            // 두 번째 오브젝트 (x축으로 이동)
            Attack5BatMove secondObject = attackObject2.GetComponent<Attack5BatMove>();
            secondObject.moveInZAxis = false;


            NextPatternPlay();
            yield return null;
        }

        //공격 6 즉사기

        IEnumerator Attack6()
        {
            // 공중 날기 애니메이션
            animator.SetBool("IsFly", true);
            float flyHeight = 3f; // 높이
            float flyDuration = 5f; //걸리는 시간
            Vector3 startPos = transform.position;
            Vector3 endPos = startPos + new Vector3(0, flyHeight, 0);
            float elapsedTime = 0f;

            // 보스를 느린 속도로 위로 이동
            while (elapsedTime < flyDuration)
            {
                transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / flyDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(2f);
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y +2f, transform.position.z);
        
            //마법진이펙트 
            GameObject bossEffectGo = Instantiate(attack6BossEffect, spawnPosition, attack6BossEffect.transform.rotation);
            bossEffectGo.transform.parent = transform;
            Destroy(bossEffectGo, 14f);
            // 애니메이션 시전 시간
            yield return new WaitForSeconds(5f);
            // 안전지대 위험구역 생성
            attack6Range.SetActive(true);

            int randomIndex;
            int safeIndex;
            randomIndex = Random.Range(0, 3);
            safeIndex = randomIndex;
            safeRanges[safeIndex].SetActive(true);
            // 안전지대 플레이어 무적, 그밖에 즉사

            




            // 기모으기 5초 
            yield return new WaitForSeconds(4f);
            // 메테오 시전
            GameObject Meteors = Instantiate(meteorPrefab, meteorPrefab.transform.position, meteorPrefab.transform.rotation);
            Destroy(Meteors, 5f);

            // 6초 이후 안전지대 위험구역 끄기, 보스 내려오기 
            yield return new WaitForSeconds(6f);
          
            attack6Range.SetActive(false);
            safeRanges[safeIndex].SetActive(false);

            // 보스가 다시 내려오게 하기
            startPos = transform.position;
            endPos = startPos - new Vector3(0, flyHeight, 0);
            elapsedTime = 0f;

            while (elapsedTime < flyDuration)
            {
                transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / flyDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            animator.SetBool("IsFly", false);

            NextPatternPlay();
            yield return null;
        }

        void StraightMove()
        {

        }


        void NextPatternPlay()
        {
            // 패턴을 순환
            nextPattern = (nextPattern % 6) + 1;

            // 특정 패턴에서만 RandomTeleport 실행
            if (nextPattern == 1 || nextPattern == 2)
            {
                StartCoroutine(RandomTeleport());
            }

            transform.LookAt(player.transform);

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
                case 5:
                    StartCoroutine(Attack5());
                    Debug.Log("5번패턴실행");
                    break;
                case 6:
                    StartCoroutine(Attack6());
                    Debug.Log("6번패턴실행");
                    break;
            }
        }

    }
}
