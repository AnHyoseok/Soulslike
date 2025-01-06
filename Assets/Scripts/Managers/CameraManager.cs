using System.Collections.Generic;
using UnityEngine;

namespace Managers
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

            // 카메라와 플레이어 사이에 다른 오브젝트가 있는지 검사
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
        void SetRendererF(RaycastHit hit)
        {
            // 충돌 오브젝트의 Tag가 Wall 인 경우
            if (hit.transform.gameObject.tag == "Wall")
            {
                // hit 오브젝트의 부모
                Transform hitParent = hit.transform.parent;
                // 자식 오브젝트들 비활성화
                Renderer[] renderers = hitParent.gameObject.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    if (!prevRayObj.Contains(renderer))
                    {
                        prevRayObj.Add(renderer); // 새로운 렌더러만 추가
                    }
                    // TODO :: shader 효과로 투명해지는 효과로
                    renderer.enabled = false; // 비활성화
                }
            }
        }

        // Renderer 활성화
        void SetRendererT()
        {
            List<Renderer> toRemove = new List<Renderer>();

            // 이전에 비활성화된 오브젝트들의 렌더러를 활성화
            foreach (Renderer prevRenderer in prevRayObj)
            {
                // TODO :: shader 효과로 투명해지는 효과로
                prevRenderer.enabled = true;
                toRemove.Add(prevRenderer); // 활성화된 렌더러는 삭제할 리스트에 추가
            }

            // 순회 후 항목 제거
            foreach (Renderer renderer in toRemove)
            {
                prevRayObj.Remove(renderer); // 렌더러를 리스트에서 제거
            }
        }
    }
}
