using BS.State;
using UnityEngine;
using DG.Tweening;

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

        public float attackTime = 1f;                       // 연계 공격 가능 시간

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
        // 키 입력 처리
        void HandleInput()
        {
            // 마우스 우클릭 이동
            if (Input.GetMouseButton(0))
            {
                // BlockingAnim 진행중에는 Return 하도록
                if (ps.isBlockingAnim) return;

                ps.isAttack = true;

                // TODO :: CursorManager에서 반환하면 좋을듯
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