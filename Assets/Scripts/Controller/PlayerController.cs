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
    /// Player�� ��Ʈ��
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        // TODO :: newinput ����غ���
        // TODO :: ScriptableObject�� ����غ��� (movespeed)
        // TODO :: MoveScript�� ���� ���� ����? ���?
        #region Variables
        // Camera
        public Camera mainCamera;                           // Camera ����

        // Debug
        public Color rayColor = Color.red;                  // Ray ��
        public float rayDuration = 1f;                      // Ray ���� �ð�

        // Move
        private Vector3 targetPosition;                     // �̵� ��ǥ ����
        private bool isMoving = false;                      // �̵� ���� 
        public float moveSpeed = 5f;                        // SD �̵� �ӵ�
        public float inGameMoveSpeed;                       // BD �̵� �ӵ�
        public float rotationDuration = 0.1f;               // ȸ�� ���� �ð�

        // Dash
        public bool isDashing = false;                      // �뽬 ������
        public float dashDuration = 0.2f;                   // �뽬 �ð�
        public float dashCoolTime = 3f;                     // SD �뽬 ��Ÿ��
        public float currentDashCoolTime = 0f;              // BD �뽬 ��Ÿ��
        public float dashDistance = 5f;                     // �뽬 �Ÿ�
        public bool isInvincible = false;                   // ���� ����
        public float invincibilityDuration = 0.5f;          // ���� �ð�

        // State
        private PlayerStateMachine stateMachine;

        // UI
        public TextMeshProUGUI dashCoolTimeText;

        // TODO :: ��ų����Ʈ ����� Dictionary �������� Ű�ڵ� : �̸�, ��Ÿ��, ����Լ�
        // �ش� ��ų����Ʈ�� �����Ҷ� �������� �ڷ�ƾ���� ��Ÿ�Ӱ���
        // ����Լ� - ������ ȣ���ϴ� �Լ� - ��Ÿ�� ���� �Լ� ���·� ����
        // TODO :: CS ������ �ݿ������ұ� ?
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

        // Ű �Է� ó��
        void HandleInput()
        {
            // TODO :: ����Ű�� ����� �̵��� �����غ���
            // ���콺 ��Ŭ�� �̵�
            if (Input.GetMouseButton(1))
            {
                // TODO :: CursorManager���� ��ȯ�ϸ� ������
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
        // ��ų ����
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
        // TODO :: DOTween �����غ���
        // Player �̵�
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

        // DoTween ȸ�� ó��
        void RotatePlayer()
        {
            // ��ǥ ȸ���� ���
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.DORotateQuaternion(targetRotation, rotationDuration);
        }

        // Player ���� ����
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
        
        // Player �ӵ� ����
        void SetMoveSpeed(float rate)
        {
            inGameMoveSpeed = moveSpeed * rate;
        }

        // �뽬
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

        // �뽬 ��
        void EndDash()
        {
            isDashing = false;
            stateMachine.ChangeState(stateMachine.IdleState);
        }

        // �뽬 �� �������� => TODO :: �ǰݸ鿪���� �ٲܼ������� �ٲ���
        void DisableInvincibility()
        {
            isInvincible = false;
        }

        // �뽬 ��Ÿ��
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

// TODO :: �÷��̾� �ٴ��� ������ IDLE�� �̵��ϴ� ���� FIX