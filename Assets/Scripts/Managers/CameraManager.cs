using System.Collections.Generic;
using UnityEngine;

namespace Managers
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
        RaycastHit hit;

        #endregion
        private void Start()
        {
            player = gameObject.transform.parent;
            mainCamera = Camera.main;
        }

        void Update()
        {
            transform.LookAt(player);

            // ī�޶�� �÷��̾� ���̿� �ٸ� ������Ʈ�� �ִ��� �˻�
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
        void SetRendererF(RaycastHit hit)
        {
            // �浹 ������Ʈ�� Tag�� Wall �� ���
            if (hit.transform.gameObject.tag == "Wall")
            {
                // hit ������Ʈ�� �θ�
                Transform hitParent = hit.transform.parent;
                // �ڽ� ������Ʈ�� ��Ȱ��ȭ
                Renderer[] renderers = hitParent.gameObject.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    if (!prevRayObj.Contains(renderer))
                    {
                        prevRayObj.Add(renderer); // ���ο� �������� �߰�
                    }
                    // TODO :: shader ȿ���� ���������� ȿ����
                    renderer.enabled = false; // ��Ȱ��ȭ
                }
            }
        }

        // Renderer Ȱ��ȭ
        void SetRendererT()
        {
            List<Renderer> toRemove = new List<Renderer>();

            // ������ ��Ȱ��ȭ�� ������Ʈ���� �������� Ȱ��ȭ
            foreach (Renderer prevRenderer in prevRayObj)
            {
                // TODO :: shader ȿ���� ���������� ȿ����
                prevRenderer.enabled = true;
                toRemove.Add(prevRenderer); // Ȱ��ȭ�� �������� ������ ����Ʈ�� �߰�
            }

            // ��ȸ �� �׸� ����
            foreach (Renderer renderer in toRemove)
            {
                prevRayObj.Remove(renderer); // �������� ����Ʈ���� ����
            }
        }
    }
}
