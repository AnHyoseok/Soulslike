using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BS.Enemy.Set
{
    [System.Serializable]
    public struct CastInfo
    {
        public bool Hit;               // �¾Ҵ��� ����
        public Vector3 Point;          // ���� ����
        public float Distance;         // ���� �Ÿ�
    }

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class FieldView : MonoBehaviour
    {
        [Header("Circle")]
        [Range(0, 30)]
        [SerializeField] private float viewRange = 10f;    // ����
        [Range(0, 360)]
        [SerializeField] private float viewAngle = 90f;    // ����

        [Header("Target")]
        [SerializeField] private LayerMask obstacleMask;   // ��ֹ� ���
        [SerializeField] private LayerMask targetMask;     // Ÿ�� ���

        [Header("Mesh")]
        [SerializeField] private Vector3 offset;           // ��ġ ������ ����
        [Range(0.1f, 1f)]
        [SerializeField] private float angleStep = 1f;     // ���� ǥ�õ� ����. ���� ���� ��������

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

            // ù ��° ������ (�߽���)
            viewPoints.Add(Vector3.zero + offset);

            // ��ä�� ���� ���
            int stepCount = Mathf.RoundToInt(viewAngle / angleStep);
            float startAngle = -viewAngle * 0.5f + transform.eulerAngles.y; // ȸ�� �� ����

            for (int i = 0; i <= stepCount; i++)
            {
                float currentAngle = startAngle + (i * angleStep);
                CastInfo castInfo = GetCastInfo(currentAngle);

                // Transform ��ǥ�迡 �°� ��ȯ
                viewPoints.Add(transform.InverseTransformPoint(castInfo.Point));
            }

            // Mesh ����
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
                triangles[i * 3] = 0;         // ������
                triangles[i * 3 + 1] = i + 1; // ù ��° ������
                triangles[i * 3 + 2] = i + 2; // �� ��° ������
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