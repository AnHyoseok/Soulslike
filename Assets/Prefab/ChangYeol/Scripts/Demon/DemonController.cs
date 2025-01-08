using System.Collections.Generic;
using UnityEngine;

namespace BS.Demon
{
    public enum DEMON
    {
        Idle,
        Attack01,
        Attack02,
        Attack03,
        GetDamaged,
        Die
    }
    public class DemonController : MonoBehaviour
    {
        public DEMON currentState = DEMON.Idle; // 현재 상태
        private Animator animator; // 애니메이터
        public Transform player; // 플레이어 참조
        public float attackRange = 3f; // 공격 범위
        public float moveSpeed = 2f; // 이동 속도
        public float[] attackCooldown; //쿨타임
        public float teleportCooldown = 3f; // 텔레포트 쿨타임
        public Transform[] teleportPoints; // 텔레포트 가능한 지점들
        public float health = 100f;     //체력
        public GameObject[] effect;
        private float currentTime;
        private float[] lastAttackTime = new float[3];
        private float lastTeleportTime = -Mathf.Infinity; // 마지막 텔레포트 시간
        private List<DEMON> demons = new List<DEMON>() { DEMON.Attack01, DEMON.Attack02, DEMON.Attack03 };

        private int index = 0;
        private void Start()
        {
            //참조
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            switch (currentState)
            {
                case DEMON.Idle:
                    HandleIdleState();
                    break;
                case DEMON.Attack01:
                    lastAttackTime[0] = Time.time;
                    HandleAttackStateOne();
                    break;
                case DEMON.Attack02:
                    lastAttackTime[1] = Time.time;
                    HandleAttackStateTwo();
                    break;
                case DEMON.Attack03:
                    lastAttackTime[2] = Time.time;
                    HandleAttackStateThree();
                    break;
                case DEMON.GetDamaged:
                    HandleGetDamagedState();
                    break;
                case DEMON.Die:
                    HandleDieState();
                    break;
            }
        }

        // 상태 전환 메서드
        public void ChangeState(DEMON newState)
        {
            if (currentState == newState) return;

            currentState = newState;

            // 상태에 따른 애니메이션 재생
            animator.SetTrigger(newState.ToString());
        }

        // 상태 처리 메서드들
        private void HandleIdleState()
        {
            if(index == 3)
            {
                index = 0;
            }
            if (Time.time - lastAttackTime[index] >= attackCooldown[index])
            {
                Debug.Log("sss");
                ChangeState(demons[index]); // 공격 상태로 전환
                return;
            }
            else
            {
                ChangeState(DEMON.Idle);
            }
        }

        private void HandleAttackStateOne()
        {
            if (index > 0) return;
            DemonPattern pattern = GetComponent<DemonPattern>();
            pattern.SpawnObjects();
            if(index == 0)
            {
                index++;
                ChangeState(DEMON.Idle);
            }

        }
        private void HandleAttackStateTwo()
        {
            if (index > 1) return;
            // 공격 쿨타임 확인
            PerformAttack("Two");
            if (index == 1)
            {
                index++;
                ChangeState(DEMON.Idle);
            }
        }
        private void HandleAttackStateThree()
        {
            if (index > 2) return;
            // 공격 쿨타임 확인
            PerformAttack("three");
            if (index == 2)
            {
                index++;
                ChangeState(DEMON.Idle);
            }
        }
        private void HandleGetDamagedState()
        {
            // 일정 시간 후 다시 Idle 상태로 전환
            Invoke(nameof(ResetToIdle), 1f);
        }

        private void HandleDieState()
        {
            // 사망 처리 (애니메이션 완료 후 파괴)
            Destroy(gameObject, 2f);
        }

        private void ResetToIdle()
        {
            if (currentState == DEMON.GetDamaged)
            {
                ChangeState(DEMON.Idle);
            }
        }

        // 체력 관리
        public void TakeDamage(int damage)
        {
            if (currentState == DEMON.Die) return; // 이미 사망 상태면 무시

            health -= damage;
            if (health <= 0)
            {
                ChangeState(DEMON.Die); // 사망 상태로 전환
            }
            else
            {
                ChangeState(DEMON.GetDamaged); // 피격 상태로 전환
            }
        }
        private void PerformAttack(string name)
        {
            // 공격 로직 (예: 플레이어에게 데미지 적용)
            Debug.Log(name);

            // 애니메이션 실행
            animator.SetTrigger("Attack01");
        }

        private void PerformTeleport()
        {
            // 텔레포트 가능한 지점 중 하나로 이동
            if (teleportPoints.Length > 0)
            {
                int randomIndex = Random.Range(0, teleportPoints.Length);
                transform.position = teleportPoints[randomIndex].position;

                /*GameObject effgo = Instantiate(effect[5], transform.position, Quaternion.identity);
                Destroy(effgo );*/
                Debug.Log("Teleported!");
            }

            // 텔레포트 애니메이션 실행 (선택 사항)
            animator.SetTrigger("Teleport");
        }
    }


}