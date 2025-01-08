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
        // TODO :: MoveScript를 따로 만들어서 적용? 상속?
        #region Variables
        // Camera
        public Camera mainCamera;                           // Camera 변수

        // Debug
        public Color rayColor = Color.red;                  // Ray 색
        public float rayDuration = 1f;                      // Ray 지속 시간

        // Move
        private Vector3 targetPosition;                     // 이동 목표 지점
        private bool isMoving = false;                      // 이동 여부 
        public float moveSpeed = 5f;                        // SD 이동 속도
        public float inGameMoveSpeed;                       // BD 이동 속도
        public float rotationDuration = 0.1f;               // 회전 지속 시간

        // Dash
        public bool isDashing = false;                      // 대쉬 중인지
        public float dashDuration = 0.2f;                   // 대쉬 시간
        public float dashCoolTime = 3f;                     // SD 대쉬 쿨타임
        public float currentDashCoolTime = 0f;              // BD 대쉬 쿨타임
        public float dashDistance = 5f;                     // 대쉬 거리
        public bool isInvincible = false;                   // 무적 인지
        public float invincibilityDuration = 0.5f;          // 무적 시간

        // State
        private PlayerStateMachine stateMachine;

        // UI
        public TextMeshProUGUI dashCoolTimeText;

        // TODO :: 스킬리스트 만들고 Dictionary 관리하자 키코드 : 이름, 쿨타임, 기능함수
        // 해당 스킬리스트를 시작할때 가져오고 코루틴으로 쿨타임관리
        // 기능함수 - 끝나고 호출하는 함수 - 쿨타임 관리 함수 형태로 구현
        // TODO :: CS 파일을 반영가능할까 ?
        private Dictionary<KeyCode, (string, float, UnityAction)> skillList;
        #endregion

        void Start()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;

            targetPosition = transform.position;
            inGameMoveSpeed = moveSpeed;

            stateMachine = GetComponent<PlayerStateMachine>();
            stateMachine.animator = transform.GetChild(0).GetComponent<Animator>();

            InitializeSkills();
        }

        void InitializeSkills()
        {
            skillList = new Dictionary<KeyCode, (string, float, UnityAction)>
            {
                { KeyCode.Space, ("Dash", dashCoolTime, DoDash) }
            };
        }

        void Update()
        {
            MoveToTarget();
            HandleInput();
        }

        // 키 입력 처리
        void HandleInput()
        {
            // TODO :: 방향키를 사용한 이동도 구현해보자
            // 마우스 우클릭 이동
            if (Input.GetMouseButton(1))
            {
                // TODO :: CursorManager에서 반환하면 좋을듯
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray);

                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.gameObject.CompareTag("Ground"))
                    {
                        targetPosition = hit.point;
                        isMoving = true;
                        RotatePlayer();

                        Debug.DrawRay(ray.origin, ray.direction * hit.distance, rayColor, rayDuration);
                        break;
                    }
                }
            }
            foreach (var skill in skillList)
            {
                if (Input.GetKeyDown(skill.Key))
                {
                    ExecuteSkill(skill.Key);
                }
            }
        }
        // 스킬 시전
        void ExecuteSkill(KeyCode key)
        {
            if (skillList.TryGetValue(key, out var skill))
            {
                if (currentDashCoolTime <= 0f)
                {
                    skill.Item3.Invoke();
                }
            }
        }
        // TODO :: DOTween 적용해보자
        // Player 이동
        void MoveToTarget()
        {
            if (isMoving && !isDashing)
            {
                SetMoveState();
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, inGameMoveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    isMoving = false;
                    stateMachine.ChangeState(stateMachine.IdleState);
                }
            }
        }

        // DoTween 회전 처리
        void RotatePlayer()
        {
            // 목표 회전값 계산
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.DORotateQuaternion(targetRotation, rotationDuration);
        }

        // Player 상태 변경
        void SetMoveState()
        {
            if (Input.GetKey(KeyCode.C))
            {
                SetMoveSpeed(0.5f);
                stateMachine.ChangeState(stateMachine.WalkState);
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                SetMoveSpeed(2);
                stateMachine.ChangeState(stateMachine.SprintState);
            }
            else
            {
                SetMoveSpeed(1);
                stateMachine.ChangeState(stateMachine.RunState);
            }
        }
        
        // Player 속도 변경
        void SetMoveSpeed(float rate)
        {
            inGameMoveSpeed = moveSpeed * rate;
        }

        // 대쉬
        void DoDash()
        {
            if (!isDashing && currentDashCoolTime<= 0f)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform.CompareTag("Ground"))
                    {
                        isDashing = true;
                        isInvincible = true;

                        Vector3 dashDirection = (hit.point - transform.position).normalized;
                        Vector3 dashTarget = transform.position + dashDirection * dashDistance;
                        stateMachine.ChangeState(stateMachine.SprintState);
                        targetPosition = dashTarget;
                        RotatePlayer();

                        transform.DOMove(dashTarget, dashDuration)
                            .SetEase(Ease.OutQuad)
                            .OnComplete(() => { EndDash(); });

                        Invoke(nameof(DisableInvincibility), invincibilityDuration);
                        StartCoroutine(DashCooldownRoutine());
                    }
                }
            }
        }

        // 대쉬 끝
        void EndDash()
        {
            isDashing = false;
            stateMachine.ChangeState(stateMachine.IdleState);
        }

        // 대쉬 끝 무적해제 => TODO :: 피격면역으로 바꿀수있으면 바꾸자
        void DisableInvincibility()
        {
            isInvincible = false;
        }

        // 대쉬 쿨타임
        IEnumerator DashCooldownRoutine()
        {
            currentDashCoolTime = dashCoolTime;
            while (currentDashCoolTime > 0f)
            {
                currentDashCoolTime -= Time.deltaTime;
                dashCoolTimeText.text = Mathf.Max(0, currentDashCoolTime).ToString("F1");
                yield return null;
            }
        }
    }
}

// TODO :: 플레이어 바닥을 찍으면 IDLE로 이동하는 버그 FIX