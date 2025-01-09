using System.Collections;
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
        [HideInInspector]public Animator animator; // 애니메이터
        public Transform player; // 플레이어 참조
        public float attackRange = 3f; // 공격 범위
        public float moveSpeed = 2f; // 이동 속도
        public float[] attackCooldown; //쿨타임
        public float teleportCooldown; // 텔레포트 쿨타임
        public Transform[] teleportPoints; // 텔레포트 가능한 지점들
        public float health = 100f;     //체력
        public GameObject[] effect;
        [HideInInspector]public float[] lastAttackTime = new float[3];
        private List<DEMON> demons = new List<DEMON>() { DEMON.Attack01, DEMON.Attack02, DEMON.Attack03 };

        private int index;
        private bool isTel = false;
        private float teleportTimer = 0f;
        private void Start()
        {
            //참조
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            teleportTimer += Time.deltaTime;
            switch (currentState)
            {
                case DEMON.Idle:
                    HandleIdleState();
                    break;
                case DEMON.Attack01:
                    HandleAttackState();
                    break;
                case DEMON.Attack02:
                    HandleAttackState();
                    break;
                case DEMON.Attack03:
                    HandleAttackState();
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
            ResetTriggers();
            index = Random.Range(0, demons.Count);
            if (Time.time - lastAttackTime[index] >= attackCooldown[index])
            {
                switch (index)
                {
                    case 0:
                        ChangeState(demons[0]);
                        break;
                    case 1:
                        ChangeState(demons[1]);
                        break;
                    case 2:
                        ChangeState(demons[2]);
                        break;
                }
            }
            else if(!isTel && teleportTimer >= teleportCooldown)
            {
                PerformTeleport();
                teleportTimer = 0;
                ChangeState(DEMON.Idle);
            }
            else
            {
                ChangeState(DEMON.Idle);
                isTel = false;
            }
        }

        private void HandleAttackState()
        {
            ChangeState(DEMON.Idle);
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

        // 애니메이션 초기화
        public void ResetTriggers()
        {
            animator.ResetTrigger("Attack01");
            animator.ResetTrigger("Attack02");
            animator.ResetTrigger("Attack03");
            animator.ResetTrigger("Idle");
            animator.ResetTrigger("Teleport");
            animator.ResetTrigger("GetDamaged");
        }
        void PerformTeleport()
        {
            isTel = true;
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