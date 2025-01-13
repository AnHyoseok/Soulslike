using BS.State;
using UnityEngine;
using DG.Tweening;

namespace BS.Player
{
    /// <summary>
    /// Player�� ���� ��Ʈ��
    /// </summary>
    public class PlayerAttackController : MonoBehaviour
    {
        #region Variables
        // Camera
        public Camera mainCamera;                           // Camera ����

        // rotation
        public float rotationDuration = 0.1f;               // ȸ�� ���� �ð�

        public float attackTime = 1f;                       // ���� ���� ���� �ð�

        // State
        PlayerState ps;
        PlayerStateMachine playerStateMachine;
        #endregion

        void Start()
        {
            ps = PlayerState.Instance;
            playerStateMachine = FindFirstObjectByType<PlayerStateMachine>();

            if (mainCamera == null)
                mainCamera = Camera.main;
        }

        void Update()
        {
            HandleInput();
            calculateTime();
        }

        #region Input
        // Ű �Է� ó��
        void HandleInput()
        {
            // ���콺 ��Ŭ�� �̵�
            if (Input.GetMouseButton(0))
            {
                // BlockingAnim �����߿��� Return �ϵ���
                if (ps.isBlockingAnim) return;

                ps.isAttack = true;

                // TODO :: CursorManager���� ��ȯ�ϸ� ������
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray);

                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.gameObject.CompareTag("Ground"))
                    {
                        ps.targetPosition = hit.point;
                        RotatePlayer();
                        playerStateMachine.ChangeState(playerStateMachine.AttackState);
                        break;
                    }
                }
            }
        }
        #endregion

        void calculateTime()
        {
            if (ps.isAttack)
            {
                attackTime -= Time.deltaTime;
            }

            if (attackTime <= 0f)
            {
                ps.isAttack = false;
                EndAttack();
            }
        }
        void EndAttack()
        {
            if(!ps.isAttack && !ps.isMoving)
            {
                attackTime = 1f;
                playerStateMachine.ChangeState(playerStateMachine.IdleState);
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
    }
}

// MEMO :: ������, ��¡����, �Ȳ�ġ