using BS.State;
using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace BS.Player
{
    public class PlayerMoveController : PlayerController
    {
        // Dash 관련 변수
        [Header("Dash Settings")]
        public float dashDuration = 0.2f;                   // 대쉬 지속 시간
        public float dashCoolTime = 3f;                     // 대쉬 쿨타임
        public float dashDistance = 5f;                     // 대쉬 거리
        public float invincibilityDuration = 0.5f;          // 무적 지속 시간

        private static readonly string IS_MOVING = "IsMoving";
        private static readonly string IS_RUNNING = "IsRun";
        private static readonly string IS_WALKING = "IsWalking";
        private static readonly string IS_SPRINTING = "IsSprinting";
        private static readonly string IS_ATTACKING = "IsAttacking";
        private static readonly string IS_DASH = "IsDash";
        private static readonly string DO_RUN = "DoRun";
        private static readonly string DO_WALK = "DoWalk";
        private static readonly string DO_SPRINT = "DoSprint";

        protected override void Awake()
        {
            base.Awake();
            if (!PlayerSkillController.skillList.ContainsKey("Space"))
            {
                PlayerSkillController.skillList.Add("Space", new Skill("Dash", dashCoolTime, DoDash));
            }
        }

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

                if (!psm.animator.GetBool(IS_MOVING))
                {
                    psm.animator.SetBool(IS_MOVING, true);
                    psm.animator.SetTrigger(DO_RUN);
                    psm.animator.SetBool(IS_RUNNING, true);
                }
            }
            SetMoveState();
        }

        // Player 이동
        private void MoveToTargetPos()
        {
            if (psm.animator.GetBool(IS_MOVING) && !ps.isBlockingAnim && !ps.isUppercuting &&
                !ps.isBackHandSwinging && !ps.isChargingPunching && !psm.animator.GetBool(IS_ATTACKING))
            {
                transform.position = Vector3.MoveTowards(transform.position, ps.targetPosition, ps.inGameMoveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, ps.targetPosition) < 0.01f)
                {
                    StopMovement();
                }
            }
        }

        private void StopMovement()
        {
            ps.isMoving = false; // 목표 지점 도달 시 이동 멈춤
            psm.animator.ResetTrigger(DO_RUN);
            psm.animator.ResetTrigger(DO_WALK);
            psm.animator.ResetTrigger(DO_SPRINT);
            psm.animator.SetBool(IS_MOVING, false);
            psm.animator.SetBool(IS_RUNNING, false);
            psm.animator.SetBool(IS_WALKING, false);
            psm.animator.SetBool(IS_SPRINTING, false);
        }

        // Player 상태 변경
        private void SetMoveState()
        {
            if (m_Input.C && !psm.animator.GetBool(IS_WALKING) && psm.animator.GetBool(IS_MOVING))
            {
                ChangeMoveState(0.5f, DO_WALK, IS_WALKING);
            }
            else if (m_Input.Shift && !psm.animator.GetBool(IS_SPRINTING) && psm.animator.GetBool(IS_MOVING))
            {
                if (!(m_Input.C && m_Input.Shift))
                {
                    ChangeMoveState(2f, DO_SPRINT, IS_SPRINTING);
                }
            }
            else if (!m_Input.C && !m_Input.Shift)
            {
                ResetToRunState();
            }
        }

        private void ChangeMoveState(float speedMultiplier, string trigger, string stateBool)
        {
            SetMoveSpeed(speedMultiplier);
            psm.animator.SetTrigger(trigger);
            psm.animator.SetBool(stateBool, true);
            psm.animator.SetBool(IS_WALKING, stateBool == IS_WALKING);
            psm.animator.SetBool(IS_RUNNING, stateBool == IS_RUNNING);
            psm.animator.SetBool(IS_SPRINTING, stateBool == IS_SPRINTING);
        }

        private void ResetToRunState()
        {
            SetMoveSpeed(1f);

            if (psm.animator.GetBool(IS_WALKING))
            {
                psm.animator.SetBool(IS_WALKING, false);
                psm.animator.SetTrigger(DO_RUN);
                psm.animator.SetBool(IS_RUNNING, true);
            }

            if (psm.animator.GetBool(IS_SPRINTING))
            {
                psm.animator.SetBool(IS_SPRINTING, false);
                psm.animator.SetTrigger(DO_RUN);
                psm.animator.SetBool(IS_RUNNING, true);
            }
        }

        // Player 속도 변경
        private void SetMoveSpeed(float rate)
        {
            ps.inGameMoveSpeed = moveSpeed * rate;
        }

        #region Dash
        // 대쉬
        private void DoDash()
        {
            Debug.Log("대쉬 작업중");
            return;

            if (!ps.isDashing && ps.currentDashCoolTime <= 0f && !ps.isBlockingAnim && ps.isDashable)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray);

                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.gameObject.CompareTag("Ground"))
                    {
                        StartDash(hit.point);
                        break;
                    }
                }
            }
        }

        private void StartDash(Vector3 targetPoint)
        {
            ps.isDashing = true;
            ps.isInvincible = true;

            Vector3 dashDirection = (targetPoint - transform.position).normalized;
            Vector3 dashTarget = transform.position + dashDirection * dashDistance;

            psm.prevState = psm.GetCurrentState();
            psm.ChangeState(psm.SprintState);
            ResetActionFlags();

            psm.animator.SetBool(IS_DASH, true);
            ps.targetPosition = dashTarget;

            transform.DOMove(dashTarget, dashDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(EndDash);

            Invoke(nameof(DisableInvincibility), invincibilityDuration);
        }

        private void ResetActionFlags()
        {
            ps.isUppercuting = false;
            ps.isBackHandSwinging = false;
            ps.isChargingPunching = false;
        }

        private void EndDash()
        {
            psm.ChangeState(psm.prevState);
            psm.animator.SetBool(IS_DASH, false);
        }

        private void DisableInvincibility()
        {
            ps.isInvincible = false;
            ps.isDashing = false;
        }
        #endregion
    }
}
