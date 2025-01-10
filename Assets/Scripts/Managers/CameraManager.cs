using BS.State;
using System.Collections.Generic;
using UnityEngine;

namespace BS.Managers
{
    /// <summary>
    /// Camera 관련 Script
    /// </summary>
    public class CameraManager : MonoBehaviour
    {
        #region Variables
        public Transform player;  // 플레이어 오브젝트
        public Camera mainCamera; // 카메라
        public List<Renderer> prevRayObj; // 이전에 비활성화된 오브젝트들
        public Renderer lastRayObj;
        public Vector3 offset = new Vector3(0f, 10f, -10f); // 플레이어와의 거리
        public float followSpeed = 5f; // 카메라 이동 속도
        private PlayerStateMachine playerStateMachine;
        RaycastHit hit;
        #endregion

        // TODO :: Shader로 가림막의 투명도를 조정해보자
        // TODO :: cinemachine을 사용해보자

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

        // 플레이어를 따라다니는 로직
        private void FollowPlayer()
        {
            // 카메라 목표 위치 계산 (플레이어 위치 + 오프셋)
            Vector3 targetPosition = player.position + offset;

            // 카메라 위치를 목표 위치로 부드럽게 이동
            mainCamera.transform.position = Vector3.Lerp(
                mainCamera.transform.position,
                targetPosition,
                followSpeed * Time.deltaTime
            );

            // 플레이어를 바라보도록 회전
            mainCamera.transform.LookAt(player);
        }

        // 카메라와 플레이어 사이의 오브젝트 투명 처리
        private void HandleObjectVisibility()
        {
            Vector3 directionToPlayer = player.position - mainCamera.transform.position;

            if (Physics.Raycast(mainCamera.transform.position, directionToPlayer, out hit))
            {
                Renderer hitRenderer = hit.transform.GetComponent<Renderer>();
                if (lastRayObj != hitRenderer)
                {
                    // 이전에 비활성화된 오브젝트들의 렌더러 활성화
                    SetRendererT();

                    // 새로운 충돌이 있는 경우 비활성화
                    SetRendererF(hit);

                    lastRayObj = hitRenderer;
                }
            }
        }

        // Renderer 비활성화
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

                    // TODO: shader 효과로 투명해지는 효과 추가
                    renderer.enabled = false;
                }
            }
        }

        // Renderer 활성화
        private void SetRendererT()
        {
            List<Renderer> toRemove = new List<Renderer>();

            foreach (Renderer prevRenderer in prevRayObj)
            {
                // TODO: shader 효과로 투명해지는 효과 추가
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
