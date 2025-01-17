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
        public float _comboableTime = 1f;                  // SD 연계 공격 가능 시간
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
                //Debug.Log("TESTATTACK");
                // BlockingAnim 진행중에는 Return 하도록
                if (ps.isBlockingAnim || ps.isDashing) return;

                bool test = false;
                //if (!ps.isAttack)

                isAttackable = true;
                ps.isMoving = false;
                ps.isMovable = false;
                comboableTime = _comboableTime;

                // TODO :: CursorManager에서 반환하면 좋을듯
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray);

                foreach (RaycastHit hit in hits)
                {
                    if (test)
                    {
                        Debug.Log("HIT go = " + hit.transform.gameObject.name);
                    }
                    if (hit.transform.gameObject.CompareTag("Ground"))
                    {
                        if (test)
                        {
                            Debug.Log("TEST2");
                        }

                        ps.targetPosition = hit.point;
                        RotatePlayer();

                        // 공격 가능한 경우
                        //TODO :: 하드코딩
                        if (animator.GetFloat("StateTime") >= 0.15f && isAttackable)
                        {
                            // 공격 Trigger 발동
                            psm.animator.SetInteger("ComboAttack", ps.ComboAttackIndex);
                            psm.ChangeState(psm.AttackState);
                        }
                        break;
                    }
                }
                if (!psm.animator.GetBool("IsAttacking"))
                {
                    psm.animator.SetBool("IsAttacking", true);
                    test = true;
                    //Debug.Log("TEST1 = " + psm.animator.GetBool("IsAttacking"));
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
            //if (ps.isAttack) return;
            if (psm.animator.GetBool("IsAttacking")) return;
            transform.DOKill(complete: false); // 트랜스폼과 관련된 모든 트윈 제거 (완료 콜백은 실행되지 않음)

            // 목표 회전값 계산
            Vector3 direction = (ps.targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.DORotateQuaternion(targetRotation, rotationDuration)
                        .SetAutoKill(true)
                        .SetEase(Ease.InOutSine)
                        .OnComplete(() =>
                        {

                        });
        }
    }
}

// MEMO :: 어퍼컷, 차징펀지, 팔꿈치