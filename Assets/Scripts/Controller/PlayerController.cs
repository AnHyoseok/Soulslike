using BS.State;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace BS.Player
{
    /// <summary>
    /// Player를 컨트롤
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        // TODO :: newinput 사용해보자
        // TODO :: ScriptableObject를 사용해보자 (movespeed)
        #region Variables
        // Camera
        public Camera mainCamera;                           // Camera 변수

        // Debug
        public Color rayColor = Color.red;                  // Ray 색
        public float rayDuration = 1f;                      // Ray 지속 시간
        private Vector3? gizmoPosition;                     // Nullable로 선언
        Animator animator;

        // Move
        public float moveSpeed = 5f;                        // SD 이동 속도
        public float rotationDuration = 0.1f;               // 회전 지속 시간

        // Dash
        public float dashDuration = 0.2f;                   // 대쉬 시간
        public float dashCoolTime = 3f;                     // SD 대쉬 쿨타임
        public float dashDistance = 5f;                     // 대쉬 거리
        public float invincibilityDuration = 0.5f;          // 무적 시간

        // State
        PlayerState ps;
        PlayerStateMachine psm;

        // UI
        public TextMeshProUGUI dashCoolTimeText;

        // 보간 속도
        public float lerpSpeed = 10f;
        #endregion
        protected virtual void Start()
        {
            ps = PlayerState.Instance;
            psm = PlayerStateMachine.Instance;

            //playerStateMachine = FindFirstObjectByType<PlayerStateMachine>();
            //playerStateMachine.animator = transform.GetChild(0).GetComponent<Animator>();

            if (mainCamera == null)
                mainCamera = Camera.main;

            ps.targetPosition = transform.position;
            ps.inGameMoveSpeed = moveSpeed;

            PlayerSkillController.skillList.Add(KeyCode.Space, ("Dash", dashCoolTime, DoDash));
        }

        protected virtual void Update()
        {
            MoveToTarget();
            HandleInput();
            RotatePlayer();
        }
        protected virtual void FixedUpdate()
        {
            SetPlayerYPos();
        }


        #region Input
        // 키 입력 처리
        void HandleInput()
        {
            // TODO :: 방향키를 사용한 이동도 구현해보자
            // 마우스 우클릭 이동
            if (Input.GetMouseButton(1))
            {
                ps.isMoving = true;
                //SetMoveState();
                // BlockingAnim 진행중에는 Return 하도록
                if (ps.isBlockingAnim) return;

                // TODO :: CursorManager에서 반환하면 좋을듯
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray);

                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.gameObject.CompareTag("Ground"))
                    {
                        ps.targetPosition = hit.point;
                        
                        gizmoPosition = hit.point; // Gizmo 위치 저장

                        Debug.DrawRay(ray.origin, ray.direction * hit.distance, rayColor, rayDuration);
                        break;
                    }
                }
            }
        }
        #endregion

        #region Move
        // TODO :: DOTween 적용해보자
        // Player 이동
        void MoveToTarget()
        {
            //if (ps.isAttack) return;
            if (psm.animator.GetBool("IsAttacking")) return;
            if (ps.isMoving && !ps.isDashing && !ps.isBlockingAnim && !ps.isUppercut && !ps.isBackHandSwing && !ps.isChargingPunch)
            {
                SetMoveState();
                transform.position = Vector3.MoveTowards(transform.position, ps.targetPosition, ps.inGameMoveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, ps.targetPosition) < 0.1f)
                {
                    ps.isMoving = false;
                    psm.ChangeState(psm.IdleState);
                }
            }
        }

        // DoTween 회전 처리
        void RotatePlayer()
        {
            if (psm.animator.GetBool("IsAttacking")) return;
            if (ps.isMoving && !ps.isBlockingAnim && !ps.isUppercut && !ps.isBackHandSwing && !ps.isChargingPunch)
            {
                // 목표 회전값 계산
                Vector3 direction = (ps.targetPosition - transform.position).normalized;
                direction = new Vector3(direction.x, 0, direction.z);
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                transform.DORotateQuaternion(targetRotation, rotationDuration);
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
        #endregion

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
                        ps.isUppercut = false;
                        ps.isBackHandSwing = false;
                        ps.isChargingPunch = false;
                        psm.animator.SetBool("IsDash", true);
                        ps.targetPosition = dashTarget;
                        RotatePlayer();

                        transform.DOMove(dashTarget, dashDuration)
                            .SetEase(Ease.OutQuad)
                            .OnComplete(() => { EndDash(); });

                        Invoke(nameof(DisableInvincibility), invincibilityDuration);
                        StartCoroutine(CoDashCooldown());
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

        // 대쉬 쿨타임
        IEnumerator CoDashCooldown()
        {
            ps.currentDashCoolTime = dashCoolTime;
            while (ps.currentDashCoolTime > 0f)
            {
                ps.currentDashCoolTime -= Time.deltaTime;
                if (dashCoolTimeText != null)
                {
                    dashCoolTimeText.text = Mathf.Max(0, ps.currentDashCoolTime).ToString("F1");
                }
                yield return null;
            }
        }
        #endregion

        private void OnDrawGizmos()
        {
            if (gizmoPosition.HasValue)
            {
                Gizmos.color = Color.red; // Gizmo 색상 설정
                Gizmos.DrawSphere(gizmoPosition.Value, 0.2f); // 반지름 0.2의 구체 그리기
            }
        }

        private void SetPlayerYPos()
        {
            Vector3 position = transform.position;

            if (position.y < 0)
            {
                // y 좌표를 0으로 보간
                position.y = Mathf.Lerp(position.y, 0, Time.deltaTime * lerpSpeed);
                transform.position = position;
            }
        }

    }
}

// TODO :: 플레이어 바닥을 찍으면 IDLE로 이동하는 버그 FIX