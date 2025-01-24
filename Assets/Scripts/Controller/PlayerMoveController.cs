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
        public float dashDuration = 0.5f;                   // 대쉬 지속 시간
        public float dashCoolTime = 3f;                     // 대쉬 쿨타임
        public float dashDistance = 5f;                     // 대쉬 거리
        public float invincibilityDuration = 0.5f;          // 무적 지속 시간

        private static readonly string IS_MOVING = "IsMoving";
        private static readonly string IS_RUNNING = "IsRun";
        private static readonly string IS_WALKING = "IsWalking";
        private static readonly string IS_SPRINTING = "IsSprinting";
        private static readonly string IS_ATTACKING = "IsAttacking";
        private static readonly string IS_DASH = "IsDashIng";
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
            if (m_Input.RightClick && ps.isMovable 
                && psm.animator.GetBool(IS_DASH) == false
                && psm.animator.GetBool("IsBlocking") == false)
            {
                ps.targetPosition = GetMousePosition();
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
            if (psm.animator.GetBool(IS_MOVING)
                && !ps.isUppercuting &&
                !ps.isBackHandSwinging && !ps.isChargingPunching
                && !psm.animator.GetBool(IS_ATTACKING)
                && psm.animator.GetBool(IS_DASH) == false
                && psm.animator.GetBool("IsBlocking") == false)
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
            if (psm.animator.GetBool(IS_DASH) == false
                && psm.animator.GetBool("IsBlocking") == false
                && ps.isDashable)
            {
                StartDash(GetMousePosition());
            }
        }

        //public enum MovingState
        //{
        //    None,
        //    Run,
        //    Sprint,
        //    Walk,
        //}
        //MovingState currMovingState;
        //MovingState prevMovingState;

        private void StartDash(Vector3 targetPoint)
        {
            psm.animator.SetBool(IS_DASH, true);
            ps.isInvincible = true;

            Vector3 dashDirection = (targetPoint - transform.position).normalized;
            Vector3 dashTarget = transform.position + dashDirection * dashDistance;
            ps.targetPosition = dashTarget;
            RotatePlayer();

            // TODO :: Sprint 모션으로 대쉬를 하고싶은데
            //StopMovement();
            //psm.prevState = psm.GetCurrentState();
            //psm.animator.SetTrigger(DO_DASH);
            //psm.ChangeState(psm.SprintState);
            //ResetActionFlags();
            //if (psm.animator.GetBool(IS_RUNNING) == true)
            //{
            //    psm.animator.SetBool(IS_RUNNING, true);
            //}
            //else if (psm.animator.GetBool(IS_SPRINTING) == true)
            //{
            //    psm.animator.SetBool(IS_SPRINTING, true);
            //}
            //else if (psm.animator.GetBool(IS_WALKING) == true)
            //{
            //    psm.animator.SetBool(IS_WALKING, true);
            //}
            //else
            //{
            //
            //}

            transform.DOMove(dashTarget, dashDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(EndDash)
            .SetLink(gameObject);

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
            psm.animator.SetBool(IS_DASH, false);
        }

        private void DisableInvincibility()
        {
            ps.isInvincible = false;
        }
        #endregion
    }
}
