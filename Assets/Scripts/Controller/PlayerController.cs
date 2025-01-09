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
        #region Variables
        // Camera
        public Camera mainCamera;                           // Camera ����

        // Debug
        public Color rayColor = Color.red;                  // Ray ��
        public float rayDuration = 1f;                      // Ray ���� �ð�

        // Move
        public float moveSpeed = 5f;                        // SD �̵� �ӵ�
        public float rotationDuration = 0.1f;               // ȸ�� ���� �ð�

        // Dash
        public float dashDuration = 0.2f;                   // �뽬 �ð�
        public float dashCoolTime = 3f;                     // SD �뽬 ��Ÿ��
        public float dashDistance = 5f;                     // �뽬 �Ÿ�
        public float invincibilityDuration = 0.5f;          // ���� �ð�

        // State
        PlayerState ps;
        PlayerStateMachine playerStateMachine;

        // UI
        public TextMeshProUGUI dashCoolTimeText;
        #endregion
        void Start()
        {
            ps = PlayerState.Instance;
            playerStateMachine = FindFirstObjectByType<PlayerStateMachine>();
            //playerStateMachine.animator = transform.GetChild(0).GetComponent<Animator>();

            if (mainCamera == null)
                mainCamera = Camera.main;

            ps.targetPosition = transform.position;
            ps.inGameMoveSpeed = moveSpeed;

            PlayerSkillController.skillList.Add(KeyCode.Space, ("Dash", dashCoolTime, DoDash));
        }

        void Update()
        {
            MoveToTarget();
            HandleInput();
        }

        #region Input
        // Ű �Է� ó��
        void HandleInput()
        {
            // TODO :: ����Ű�� ����� �̵��� �����غ���
            // ���콺 ��Ŭ�� �̵�
            if (Input.GetMouseButton(1))
            {
                // BlockingAnim �����߿��� Return �ϵ���
                if (ps.isBlockingAnim) return;

                // TODO :: CursorManager���� ��ȯ�ϸ� ������
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray);

                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.gameObject.CompareTag("Ground"))
                    {
                        ps.targetPosition = hit.point;
                        ps.isMoving = true;
                        RotatePlayer();

                        Debug.DrawRay(ray.origin, ray.direction * hit.distance, rayColor, rayDuration);
                        break;
                    }
                }
            }
        }
        #endregion

        #region Move
        // TODO :: DOTween �����غ���
        // Player �̵�
        void MoveToTarget()
        {
            if (ps.isMoving && !ps.isDashing && !ps.isBlockingAnim)
            {
                SetMoveState();
                transform.position = Vector3.MoveTowards(transform.position, ps.targetPosition, ps.inGameMoveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, ps.targetPosition) < 0.1f)
                {
                    ps.isMoving = false;
                    playerStateMachine.ChangeState(playerStateMachine.IdleState);
                }
            }
        }

        // DoTween ȸ�� ó��
        void RotatePlayer()
        {
            // ��ǥ ȸ���� ���
            Vector3 direction = (ps.targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.DORotateQuaternion(targetRotation, rotationDuration);
        }

        // Player ���� ����
        void SetMoveState()
        {
            if (Input.GetKey(KeyCode.C))
            {
                SetMoveSpeed(0.5f);
                playerStateMachine.ChangeState(playerStateMachine.WalkState);
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                SetMoveSpeed(2);
                playerStateMachine.ChangeState(playerStateMachine.SprintState);
            }
            else
            {
                SetMoveSpeed(1);
                playerStateMachine.ChangeState(playerStateMachine.RunState);
            }
        }

        // Player �ӵ� ����
        void SetMoveSpeed(float rate)
        {
            ps.inGameMoveSpeed = moveSpeed * rate;
        }
        #endregion

        #region Dash
        // �뽬
        void DoDash()
        {
            if (!ps.isDashing && ps.currentDashCoolTime <= 0f && !ps.isBlockingAnim)
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
                        playerStateMachine.ChangeState(playerStateMachine.SprintState);
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

        // �뽬 ��
        void EndDash()
        {
            ps.isDashing = false;
            if (ps.isMoving)
            {
                playerStateMachine.ChangeState(playerStateMachine.RunState);
            }
            else
            {
                playerStateMachine.ChangeState(playerStateMachine.IdleState);
            }
        }

        // �뽬 �� �������� => TODO :: �ǰݸ鿪���� �ٲܼ������� �ٲ���
        void DisableInvincibility()
        {
            ps.isInvincible = false;
        }

        // �뽬 ��Ÿ��
        IEnumerator CoDashCooldown()
        {
            ps.currentDashCoolTime = dashCoolTime;
            while (ps.currentDashCoolTime > 0f)
            {
                ps.currentDashCoolTime -= Time.deltaTime;
                dashCoolTimeText.text = Mathf.Max(0, ps.currentDashCoolTime).ToString("F1");
                yield return null;
            }
        }
        #endregion
    }
}

// TODO :: �÷��̾� �ٴ��� ������ IDLE�� �̵��ϴ� ���� FIX