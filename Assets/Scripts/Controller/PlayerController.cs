using UnityEngine;

namespace Player
{
    /// <summary>
    /// Player의 controll 관련 Script
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        public Camera mainCamera; // 마우스를 감지할 카메라 (기본적으로 메인 카메라 사용)
        public float moveSpeed = 5f; // 이동 속도
        private Vector3 targetPosition; // 이동 목표 위치
        private bool isMoving = false; // 이동 중 여부

        public Color rayColor = Color.red; // 레이의 색상
        public float rayDuration = 1f; // 레이가 표시되는 시간
        #endregion
        void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main; // 메인 카메라를 자동으로 할당
            }

            targetPosition = transform.position; // 초기 위치 설정
        }

        void Update()
        {
            HandleMouseInput();
            MoveToTarget();
        }

        void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(1)) // 마우스 오른쪽 버튼 클릭
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); // 마우스 위치에서 Ray 생성
                RaycastHit[] hits; // 모든 충돌된 정보를 배열로 받을 변수

                hits = Physics.RaycastAll(ray); // Ray가 충돌한 모든 오브젝트를 배열로 반환

                foreach (RaycastHit hit in hits) // 모든 충돌체 순회
                {
                    Debug.Log(hit.transform.gameObject.tag);
                    if (hit.transform.gameObject.CompareTag("Ground"))
                    {
                        targetPosition = hit.point; // 충돌한 위치를 목표 위치로 설정
                        targetPosition.y = 0f; // y값을 0으로 고정 (지면에 위치하도록)
                        isMoving = true; // 이동 시작

                        // 레이를 그리기
                        Debug.DrawRay(ray.origin, ray.direction * hit.distance, rayColor, rayDuration);
                    }
                }
            }
        }

        void MoveToTarget()
        {
            if (isMoving)
            {
                // 현재 위치에서 목표 위치로 이동
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                // 목표 위치에 도달하면 이동 중지
                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    isMoving = false;
                }
            }
        }
    }
}
