using BS.State;
using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace BS.Player
{
    public class PlayerMoveController : PlayerController
    {
        // Dash
        public float dashDuration = 0.2f;                   // 대쉬 시간
        public float dashCoolTime = 3f;                     // SD 대쉬 쿨타임
        public float dashDistance = 5f;                     // 대쉬 거리
        public float invincibilityDuration = 0.5f;          // 무적 시간

        protected override void Awake()
        {
            base.Awake();
            PlayerSkillController.skillList.Add("Space", new Skill("Dash", dashCoolTime, DoDash));
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
            if (m_Input.RightClick)
            {
                GetMousePosition();
                ps.isMoving = true;
            }
        }

        // TODO :: DOTween 적용해보자
        // TODO :: 방향키를 사용한 이동도 구현해보자
        // Player 이동
        private void MoveToTargetPos()
        {
            //if (ps.isAttack) return;
            //if (ps.isBlockingAnim) return;
            if (psm.animator.GetBool("IsAttacking")) return;
            if (ps.isMoving && !ps.isBlockingAnim && !ps.isUppercuting && !ps.isBackHandSwinging && !ps.isChargingPunching)
            {
                SetMoveState();
                transform.position = Vector3.MoveTowards(transform.position, ps.targetPosition, ps.inGameMoveSpeed * Time.deltaTime);
                RotateToTargetPos();
                if (Vector3.Distance(transform.position, ps.targetPosition) < 0.01f)
                {
                    ps.isMoving = false; // 목표 지점 도달 시 이동 멈춤
                    psm.ChangeState(psm.IdleState);
                }
            }
        }

        // Player 상태 변경
        void SetMoveState()
        {
            if (Input.GetKey(KeyCode.C))
            {
                SetMoveSpeed(0.5f);
                psm.ChangeState(psm.WalkState);
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                SetMoveSpeed(2);
                psm.ChangeState(psm.SprintState);
            }
            else
            {
                SetMoveSpeed(1);
                psm.ChangeState(psm.RunState);
            }
        }

        // Player 속도 변경
        void SetMoveSpeed(float rate)
        {
            ps.inGameMoveSpeed = moveSpeed * rate;
        }

        #region Dash
        // 대쉬
        void DoDash()
        {
            if (!ps.isDashing && ps.currentDashCoolTime <= 0f && !ps.isBlockingAnim && ps.isDashable)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray);
                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.gameObject.CompareTag("Ground"))
                    {
                        ps.isDashing = true;
                        ps.isInvincible = true;

                        Vector3 dashDirection = (hit.point - transform.position).normalized;
                        Vector3 dashTarget = transform.position + dashDirection * dashDistance;
                        if (psm.GetCurrentState() is SprintState)
                        {
                            psm.prevState = psm.SprintState;
                        }
                        psm.ChangeState(psm.SprintState);
                        ps.isUppercuting = false;
                        ps.isBackHandSwinging = false;
                        ps.isChargingPunching = false;
                        psm.animator.SetBool("IsDash", true);
                        ps.targetPosition = dashTarget;
                        //RotatePlayer();

                        transform.DOMove(dashTarget, dashDuration)
                            .SetEase(Ease.OutQuad)
                            .OnComplete(() => { EndDash(); });

                        Invoke(nameof(DisableInvincibility), invincibilityDuration);
                        //StartCoroutine(CoDashCooldown());
                    }
                }
            }
        }

        // 대쉬 끝
        void EndDash()
        {
            if (psm.GetPrevState() is RunState)
            {
                psm.ChangeState(psm.RunState);
            }
            else if (psm.GetPrevState() is IdleState)
            {
                psm.ChangeState(psm.IdleState);
            }
            else if (psm.GetPrevState() is WalkState)
            {
                psm.ChangeState(psm.WalkState);
            }
            else if (psm.GetPrevState() is SprintState)
            {
                psm.ChangeState(psm.SprintState);
            }
            psm.animator.SetBool("IsDash", false);
        }

        // 대쉬 끝 무적해제 => TODO :: 피격면역으로 바꿀수있으면 바꾸자
        void DisableInvincibility()
        {
            ps.isInvincible = false;
            ps.isDashing = false;
        }
        #endregion
    }
}
