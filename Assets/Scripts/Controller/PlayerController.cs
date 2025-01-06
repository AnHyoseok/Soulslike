using UnityEngine;

namespace Player
{
    /// <summary>
    /// Player�� controll ���� Script
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        public Camera mainCamera; // ���콺�� ������ ī�޶� (�⺻������ ���� ī�޶� ���)
        public float moveSpeed = 5f; // �̵� �ӵ�
        private Vector3 targetPosition; // �̵� ��ǥ ��ġ
        private bool isMoving = false; // �̵� �� ����

        public Color rayColor = Color.red; // ������ ����
        public float rayDuration = 1f; // ���̰� ǥ�õǴ� �ð�
        #endregion
        void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main; // ���� ī�޶� �ڵ����� �Ҵ�
            }

            targetPosition = transform.position; // �ʱ� ��ġ ����
        }

        void Update()
        {
            HandleMouseInput();
            MoveToTarget();
        }

        void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(1)) // ���콺 ������ ��ư Ŭ��
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); // ���콺 ��ġ���� Ray ����
                RaycastHit[] hits; // ��� �浹�� ������ �迭�� ���� ����

                hits = Physics.RaycastAll(ray); // Ray�� �浹�� ��� ������Ʈ�� �迭�� ��ȯ

                foreach (RaycastHit hit in hits) // ��� �浹ü ��ȸ
                {
                    Debug.Log(hit.transform.gameObject.tag);
                    if (hit.transform.gameObject.CompareTag("Ground"))
                    {
                        targetPosition = hit.point; // �浹�� ��ġ�� ��ǥ ��ġ�� ����
                        targetPosition.y = 0f; // y���� 0���� ���� (���鿡 ��ġ�ϵ���)
                        isMoving = true; // �̵� ����

                        // ���̸� �׸���
                        Debug.DrawRay(ray.origin, ray.direction * hit.distance, rayColor, rayDuration);
                    }
                }
            }
        }

        void MoveToTarget()
        {
            if (isMoving)
            {
                // ���� ��ġ���� ��ǥ ��ġ�� �̵�
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                // ��ǥ ��ġ�� �����ϸ� �̵� ����
                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    isMoving = false;
                }
            }
        }
    }
}
