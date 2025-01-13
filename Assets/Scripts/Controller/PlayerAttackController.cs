using BS.State;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace BS.Player
{
    /// <summary>
    /// Player를 공격 컨트롤
    /// </summary>
    public class PlayerAttackController : MonoBehaviour
    {
        #region Variables
        // Camera
        public Camera mainCamera;                           // Camera 변수

        // rotation
        public float rotationDuration = 0.1f;               // 회전 지속 시간

        public float comboableTime;                        // 연계 공격 가능 시간
        public float _comboableTime = 5f;                  // SD 연계 공격 가능 시간
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
        // 키 입력 처리
        void HandleInput()
        {
            // 마우스 좌클릭 공격
            if (Input.GetMouseButton(0))
            {
                // BlockingAnim 진행중에는 Return 하도록
                if (ps.isBlockingAnim) return;

                isAttackable = true;
                ps.isAttack = true;
                comboableTime = _comboableTime;

                // TODO :: CursorManager에서 반환하면 좋을듯
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray);

                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.gameObject.CompareTag("Ground"))
                    {
                        ps.targetPosition = hit.point;
                        RotatePlayer();

                        // 공격 가능한 경우
                        if (animator.GetFloat("StateTime") >= 0.2f && isAttackable)
                        {
                            // 공격 Trigger 발동
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

        // DoTween 회전 처리
        void RotatePlayer()
        {
            // 목표 회전값 계산
            Vector3 direction = (ps.targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.DORotateQuaternion(targetRotation, rotationDuration);
        }
    }
}

// MEMO :: 어퍼컷, 차징펀지, 팔꿈치