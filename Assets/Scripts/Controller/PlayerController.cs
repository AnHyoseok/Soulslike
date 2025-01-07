using BS.State;
using DG.Tweening;
using UnityEngine;

namespace BS.Player
{
    /// <summary>
    /// Player의 이동과 회전을 제어
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        #region Variables

        public Camera mainCamera;
        public float moveSpeed = 5f;
        public float inGameMoveSpeed;
        public float rotationDuration = 0.1f; // 회전 지속 시간

        private Vector3 targetPosition;
        private bool isMoving = false;

        public Color rayColor = Color.red;
        public float rayDuration = 1f;

        private PlayerStateMachine stateMachine;
        #endregion

        void Start()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;

            targetPosition = transform.position;
            inGameMoveSpeed = moveSpeed;

            stateMachine = GetComponent<PlayerStateMachine>();
            stateMachine.animator = transform.GetChild(0).GetComponent<Animator>();
        }

        void Update()
        {
            MoveToTarget();
            HandleMouseInput();
        }

        void HandleMouseInput()
        {
            // TODO :: 방향키를 사용한 이동도 구현해보자

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
                        targetPosition.y = 0f;
                        isMoving = true;

                        // 목표 회전값 계산
                        Vector3 direction = (targetPosition - transform.position).normalized;
                        Quaternion targetRotation = Quaternion.LookRotation(direction);

                        // DOTween으로 회전 처리
                        transform.DORotateQuaternion(targetRotation, rotationDuration);


                        Debug.DrawRay(ray.origin, ray.direction * hit.distance, rayColor, rayDuration);
                        break;
                    }
                }
            }
        }

        // TODO :: DOTween 적용해보자
        void MoveToTarget()
        {
            if (isMoving)
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

        void SetMoveState()
        {
            // TODO :: newinput 사용해보자
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

        // TODO :: ScriptableObject로 movespeed를 빼놓는다면?
        void SetMoveSpeed(float rate)
        {
            inGameMoveSpeed = moveSpeed;
            inGameMoveSpeed *= rate;
        }
    }
}
