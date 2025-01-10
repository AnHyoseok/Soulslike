using BS.State;
using System.Collections.Generic;
using UnityEngine;

namespace BS.Managers
{
    /// <summary>
    /// Camera ���� Script
    /// </summary>
    public class CameraManager : MonoBehaviour
    {
        #region Variables
        public Transform player;  // �÷��̾� ������Ʈ
        public Camera mainCamera; // ī�޶�
        public List<Renderer> prevRayObj; // ������ ��Ȱ��ȭ�� ������Ʈ��
        public Renderer lastRayObj;
        public Vector3 offset = new Vector3(0f, 10f, -10f); // �÷��̾���� �Ÿ�
        public float followSpeed = 5f; // ī�޶� �̵� �ӵ�
        private PlayerStateMachine playerStateMachine;
        RaycastHit hit;
        #endregion

        // TODO :: Shader�� �������� ������ �����غ���
        // TODO :: cinemachine�� ����غ���

        private void Start()
        {
            if (player == null)
            {
                playerStateMachine = FindFirstObjectByType<PlayerStateMachine>();
                if (playerStateMachine != null)
                {
                    player = playerStateMachine.transform;
                }
                else
                {
                    Debug.Log("PLAYER NULL");
                }
            }

            if (mainCamera == null)
                mainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            if (player == null)
            {
                Debug.Log("PLAYER NULL");
                return;
            }
            FollowPlayer();
            HandleObjectVisibility();
        }

        // �÷��̾ ����ٴϴ� ����
        private void FollowPlayer()
        {
            // ī�޶� ��ǥ ��ġ ��� (�÷��̾� ��ġ + ������)
            Vector3 targetPosition = player.position + offset;

            // ī�޶� ��ġ�� ��ǥ ��ġ�� �ε巴�� �̵�
            mainCamera.transform.position = Vector3.Lerp(
                mainCamera.transform.position,
                targetPosition,
                followSpeed * Time.deltaTime
            );

            // �÷��̾ �ٶ󺸵��� ȸ��
            mainCamera.transform.LookAt(player);
        }

        // ī�޶�� �÷��̾� ������ ������Ʈ ���� ó��
        private void HandleObjectVisibility()
        {
            Vector3 directionToPlayer = player.position - mainCamera.transform.position;

            if (Physics.Raycast(mainCamera.transform.position, directionToPlayer, out hit))
            {
                Renderer hitRenderer = hit.transform.GetComponent<Renderer>();
                if (lastRayObj != hitRenderer)
                {
                    // ������ ��Ȱ��ȭ�� ������Ʈ���� ������ Ȱ��ȭ
                    SetRendererT();

                    // ���ο� �浹�� �ִ� ��� ��Ȱ��ȭ
                    SetRendererF(hit);

                    lastRayObj = hitRenderer;
                }
            }
        }

        // Renderer ��Ȱ��ȭ
        private void SetRendererF(RaycastHit hit)
        {
            if (hit.transform.gameObject.tag == "Wall")
            {
                Transform hitParent = hit.transform.parent;
                Renderer[] renderers = hitParent.gameObject.GetComponentsInChildren<Renderer>();

                foreach (Renderer renderer in renderers)
                {
                    if (!prevRayObj.Contains(renderer))
                        prevRayObj.Add(renderer);

                    // TODO: shader ȿ���� ���������� ȿ�� �߰�
                    renderer.enabled = false;
                }
            }
        }

        // Renderer Ȱ��ȭ
        private void SetRendererT()
        {
            List<Renderer> toRemove = new List<Renderer>();

            foreach (Renderer prevRenderer in prevRayObj)
            {
                // TODO: shader ȿ���� ���������� ȿ�� �߰�
                prevRenderer.enabled = true;
                toRemove.Add(prevRenderer);
            }

            foreach (Renderer renderer in toRemove)
            {
                prevRayObj.Remove(renderer);
            }
        }
    }
}
