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
    public class TestAndMemo : MonoBehaviour
    {
        #region Variables
        public Camera mainCamera;
        public float moveSpeed = 5f;
        public float inGameMoveSpeed;
        public float rotationDuration = 0.1f; // 회전 지속 시간
        public float dashDistance = 5f; // 대쉬 거리
        public float dashDuration = 0.2f; // 대쉬 시간
        public float invincibilityDuration = 0.5f; // 무적 시간
        public float dashCooldown = 10f; // 대쉬 쿨타임

        private Vector3 targetPosition;
        private bool isMoving = false;
        private bool isDashing = false;
        private bool isInvincible = false;
        private float currentDashCooldown = 0f;
        private PlayerStateMachine stateMachine;

        public TextMeshProUGUI dashCooldownText;

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

        void Update()
        {
            MoveToTarget();
            HandleInput();
            UpdateSkillCooldowns();
        }

        void InitializeSkills()
        {
            skillList = new Dictionary<KeyCode, (string, float, UnityAction)>
            {
                { KeyCode.Space, ("Dash", dashCooldown, DoDash) }
            };
        }

        void HandleInput()
        {
            // 마우스 우클릭 이동
            if (Input.GetMouseButton(1))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray);

                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.gameObject.CompareTag("Ground"))
                    {
                        targetPosition = hit.point;
                        isMoving = true;
                        RotatePlayer();

                        Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 1f);
                        break;
                    }
                }
            }

            // 키 입력 처리
            foreach (var skill in skillList)
            {
                if (Input.GetKeyDown(skill.Key))
                {
                    ExecuteSkill(skill.Key);
                }
            }
        }

        void ExecuteSkill(KeyCode key)
        {
            if (skillList.TryGetValue(key, out var skill))
            {
                if (currentDashCooldown <= 0f)
                {
                    skill.Item3.Invoke();
                }
            }
        }

        void UpdateSkillCooldowns()
        {
            if (currentDashCooldown > 0f)
            {
                currentDashCooldown -= Time.deltaTime;
                dashCooldownText.text = Mathf.Max(0, currentDashCooldown).ToString("F1");
            }
        }

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
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.DORotateQuaternion(targetRotation, rotationDuration);
        }

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

        void SetMoveSpeed(float rate)
        {
            inGameMoveSpeed = moveSpeed * rate;
        }

        // Dash 기능
        void DoDash()
        {
            if (!isDashing && currentDashCooldown <= 0f)
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

                        RotatePlayer();

                        transform.DOMove(dashTarget, dashDuration)
                            .SetEase(Ease.OutQuad)
                            .OnComplete(() => { isDashing = false; stateMachine.ChangeState(stateMachine.IdleState); });

                        Invoke(nameof(DisableInvincibility), invincibilityDuration);
                        StartCoroutine(DashCooldownRoutine());
                    }
                }
            }
        }

        void DisableInvincibility()
        {
            isInvincible = false;
        }

        IEnumerator DashCooldownRoutine()
        {
            currentDashCooldown = dashCooldown;
            while (currentDashCooldown > 0f)
            {
                yield return null;
            }
        }
    }
}
