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
    public class DemonState : MonoBehaviour
    {
        public DEMON currentState = DEMON.Idle; // 현재 상태
        public Animator animator; // 애니메이터
        public Transform player; // 플레이어 참조
        public float attackRange = 3f; // 공격 범위
        public float moveSpeed = 2f; // 이동 속도
        public float attackCooldown = 5f; // 공격 쿨타임
        public float teleportCooldown = 3f; // 텔레포트 쿨타임
        public Transform[] teleportPoints; // 텔레포트 가능한 지점들
        public float health = 100f;     //체력
        public GameObject[] effect;

        private float lastAttackTime = -Mathf.Infinity; // 마지막 공격 시간
        private float lastTeleportTime = -Mathf.Infinity; // 마지막 텔레포트 시간

        private void Update()
        {
            switch (currentState)
            {
                case DEMON.Idle:
                    HandleIdleState();
                    break;
                case DEMON.Attack01:
                case DEMON.Attack02:
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
            // 플레이어와의 거리 체크
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                ChangeState(DEMON.Attack01); // 공격 상태로 전환
            }
            else
            {
                ChangeState(DEMON.Idle); // 걷기 상태로 전환
            }
        }

        private void HandleWalkState()
        {
            // 플레이어를 향해 이동
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // 공격 범위에 도달하면 공격 상태로 전환
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                ChangeState(DEMON.Attack01);
            }
        }

        private void HandleAttackState()
        {
            float currentTime = Time.time;

            // 공격 쿨타임 확인
            if (currentTime >= lastAttackTime + attackCooldown)
            {
                PerformAttack();
                lastAttackTime = currentTime;
            }
            else if (currentTime >= lastTeleportTime + teleportCooldown)
            {
                PerformTeleport();
                lastTeleportTime = currentTime;
            }
            else
            {
                // 쿨타임 대기
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
        /*public void TakeDamage(int damage)
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
        }*/
        private void PerformAttack()
        {
            // 공격 로직 (예: 플레이어에게 데미지 적용)
            Debug.Log("Attack performed!");

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

                GameObject effgo = Instantiate(effect[5], transform.position, Quaternion.identity);
                Debug.Log("Teleported!");
            }

            // 텔레포트 애니메이션 실행 (선택 사항)
            animator.SetTrigger("Teleport");
        }
    }


}