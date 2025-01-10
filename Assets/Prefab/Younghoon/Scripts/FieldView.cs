using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BS.Enemy.Set
{
    [System.Serializable]
    public struct CastInfo
    {
        public bool Hit;               // 맞았는지 여부
        public Vector3 Point;          // 맞은 지점
        public float Distance;         // 도달 거리
    }

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class FieldView : MonoBehaviour
    {
        [Header("Circle")]
        [Range(0, 30)]
        [SerializeField] private float viewRange = 10f;    // 범위
        [Range(0, 360)]
        [SerializeField] private float viewAngle = 90f;    // 각도

        [Header("Target")]
        [SerializeField] private LayerMask obstacleMask;   // 장애물 대상
        [SerializeField] private LayerMask targetMask;     // 타겟 대상

        [Header("Mesh")]
        [SerializeField] private Vector3 offset;           // 위치 보정용 벡터
        [Range(0.1f, 1f)]
        [SerializeField] private float angleStep = 1f;     // 선이 표시될 각도. 작을 수록 촘촘해짐

        private Mesh viewMesh;
        private MeshFilter meshFilter;
        [SerializeField] private Material material;
        private MeshRenderer meshRenderer;

        private void Start()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
            viewMesh = new Mesh();
            meshFilter.mesh = viewMesh;
            meshRenderer.material = material;
        }
        private void Update()
        {
            DrawFieldOfView();
        }

        private void DrawFieldOfView()
        {
            List<Vector3> viewPoints = new List<Vector3>();

            // 첫 번째 꼭짓점 (중심점)
            viewPoints.Add(Vector3.zero + offset);

            // 부채꼴 각도 계산
            int stepCount = Mathf.RoundToInt(viewAngle / angleStep);
            float startAngle = -viewAngle * 0.5f + transform.eulerAngles.y; // 회전 값 적용

            for (int i = 0; i <= stepCount; i++)
            {
                float currentAngle = startAngle + (i * angleStep);
                CastInfo castInfo = GetCastInfo(currentAngle);

                // Transform 좌표계에 맞게 변환
                viewPoints.Add(transform.InverseTransformPoint(castInfo.Point));
            }

            // Mesh 갱신
            UpdateMesh(viewPoints);
        }


        private void UpdateMesh(List<Vector3> viewPoints)
        {
            viewMesh.Clear();

            int vertexCount = viewPoints.Count;
            Vector3[] vertices = viewPoints.ToArray();
            int[] triangles = new int[(vertexCount - 2) * 3];

            for (int i = 0; i < vertexCount - 2; i++)
            {
                triangles[i * 3] = 0;         // 시작점
                triangles[i * 3 + 1] = i + 1; // 첫 번째 꼭짓점
                triangles[i * 3 + 2] = i + 2; // 두 번째 꼭짓점
            }

            viewMesh.vertices = vertices;
            viewMesh.triangles = triangles;
            viewMesh.RecalculateNormals();
        }

        private CastInfo GetCastInfo(float angle)
        {
            Vector3 dir = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
            CastInfo info;
            RaycastHit hit;

            if (Physics.Raycast(transform.position + offset, dir, out hit, viewRange, obstacleMask))
            {
                info.Hit = true;
                info.Point = hit.point;
                info.Distance = hit.distance;
            }
            else
            {
                info.Hit = false;
                info.Point = transform.position + offset + dir * viewRange;
                info.Distance = viewRange;
            }

            return info;
        }
    }
}