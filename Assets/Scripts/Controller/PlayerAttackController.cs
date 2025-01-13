using BS.State;
using UnityEngine;
using DG.Tweening;
using TMPro;

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

        public float comboableTime;                        // ���� ���� ���� �ð�
        public float _comboableTime = 5f;                  // SD ���� ���� ���� �ð�
        public bool isAttackable = false;
        
        // State
        PlayerState ps;
        PlayerStateMachine psm;

        public Animator animator;

        public TextMeshProUGUI txt1;
        public TextMeshProUGUI txt2;
        #endregion

        void Start()
        {
            comboableTime = _comboableTime;
            ps = PlayerState.Instance;
            psm = PlayerStateMachine.Instance;
            //playerStateMachine = FindFirstObjectByType<PlayerStateMachine>();

            if (mainCamera == null)
                mainCamera = Camera.main;
        }

        void Update()
        {
            calculateTime();
            txt1.text = ps.ComboAttackIndex.ToString();
            txt2.text = Mathf.RoundToInt(comboableTime).ToString();
        }
        private void FixedUpdate()
        {
            HandleInput();
            
        }
        #region Input
        // Ű �Է� ó��
        void HandleInput()
        {
            // ���콺 ��Ŭ�� ����
            if (Input.GetMouseButton(0))
            {
                // BlockingAnim �����߿��� Return �ϵ���
                if (ps.isBlockingAnim) return;

                isAttackable = true;
                ps.isAttack = true;
                comboableTime = _comboableTime;

                // TODO :: CursorManager���� ��ȯ�ϸ� ������
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray);

                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.gameObject.CompareTag("Ground"))
                    {
                        ps.targetPosition = hit.point;
                        RotatePlayer();

                        // ���� ������ ���
                        if (animator.GetFloat("StateTime") >= 0.2f && isAttackable)
                        {
                            // ���� Trigger �ߵ�
                            psm.animator.SetInteger("ComboAttack", ps.ComboAttackIndex);
                            psm.ChangeState(psm.AttackState);
                        }
                        break;
                    }
                }
            }
        }
        #endregion

        void calculateTime()
        {
            if (isAttackable)
            {
                comboableTime -= Time.deltaTime;
            }

            if (comboableTime <= 0f)
            {
                isAttackable = false;
                ps.ComboAttackIndex = 1;
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