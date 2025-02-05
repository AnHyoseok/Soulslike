using DG.Tweening;
using UnityEngine;

namespace BS.Player
{
    public class Test : PlayerController
    {
        // Dash
        public float dashDuration = 0.2f;                   // 대쉬 시간
        public float dashCoolTime = 3f;                     // SD 대쉬 쿨타임
        public float dashDistance = 5f;                     // 대쉬 거리
        public float invincibilityDuration = 0.5f;          // 무적 시간

        protected override void Awake()
        {
            base.Awake();
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            SetTargetPosition();
            MoveToTargetPos();
        }

        // Target Position 설정
        private void SetTargetPosition()
        {
            if (m_Input.RightClick && ps.isMovable)
            {
                GetMousePosition();
                RotatePlayer();
                if (animator.GetBool("IsMoving") == false)
                {
                    //SetMoveSpeed(1);
                    animator.SetBool("IsMoving", true);
                    animator.SetTrigger("DoRun");
                    animator.SetBool("IsRun", true);
                }
            }
            SetMoveState();
        }

        // TODO :: DOTween 적용해보자
        // TODO :: 방향키를 사용한 이동도 구현해보자
        // Player 이동
        private void MoveToTargetPos()
        {
            // TODO :: 이동이 불가능한 상태 분기처리 리턴
            //if (ps.isAttack) return;
            //if (ps.isBlockingAnim) return;
            //if (animator.GetBool("IsAttacking")) return;
            if ((animator.GetBool("IsMoving") == true
                //|| animator.GetBool("IsWalking") == true || animator.GetBool("IsSprinting") == true
                )
                //&& !ps.isBlockingAnim
                && !ps.isUppercuting
                && !ps.isBackHandSwinging
                && !ps.isChargingPunching
                && animator.GetBool("IsAttacking") == false
                )
            {
                transform.position = Vector3.MoveTowards(transform.position, ps.targetPosition, ps.inGameMoveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, ps.targetPosition) < 0.01f)
                {
                    ps.isMoving = false; // 목표 지점 도달 시 이동 멈춤
                    animator.ResetTrigger("DoRun");
                    animator.ResetTrigger("DoWalk");
                    animator.ResetTrigger("DoSprint");
                    animator.SetBool("IsMoving", false);
                    animator.SetBool("IsRun", false);
                    animator.SetBool("IsWalking", false);
                    animator.SetBool("IsSprinting", false);
                    //ChangeState(IdleState);
                }
            }
        }

        // Player 상태 변경
        void SetMoveState()
        {
            if (m_Input.C && animator.GetBool("IsWalking") == false
                && animator.GetBool("IsMoving") == true
                )
            {
                SetMoveSpeed(0.5f);
                animator.SetTrigger("DoWalk");
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsSprinting", false);
                animator.SetBool("IsRun", false);
                //ChangeState(WalkState);
            }
            else if (m_Input.Shift && animator.GetBool("IsSprinting") == false
                && animator.GetBool("IsMoving") == true
                )
            {
                if (m_Input.C && m_Input.Shift) return;
                SetMoveSpeed(2);
                animator.SetTrigger("DoSprint");
                animator.SetBool("IsSprinting", true);
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsRun", false);
            }
            else if (!m_Input.C && !m_Input.Shift)
            {
                SetMoveSpeed(1);
                if (animator.GetBool("IsWalking") == true)
                {
                    animator.SetBool("IsWalking", false);
                    animator.SetTrigger("DoRun");
                    animator.SetBool("IsRun", true);
                }
                if (animator.GetBool("IsSprinting") == true)
                {
                    animator.SetBool("IsSprinting", false);
                    animator.SetTrigger("DoRun");
                    animator.SetBool("IsRun", true);
                }
            }
            //animator.ResetTrigger("DoWalk");
            //animator.SetBool("IsWalking", true);
        }

        // Player 속도 변경
        void SetMoveSpeed(float rate)
        {
            ps.inGameMoveSpeed = moveSpeed * rate;
        }

    }
}
