using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UIElements;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FanShape : MonoBehaviour
{
    public float radius = 1f;         // 부채꼴의 반지름
    public float angle = 90f;        // 부채꼴의 각도
    public int segments = 10;        // 부채꼴을 이루는 세그먼트(삼각형 수)
    public Material material;
    public LayerMask layerMask;    // 탐지할 레이어 설정

    private void Start()
    {
        GenerateFanShape();
    }

    //private void OnValidate()
    //{
    //    GenerateFanShape();
    //}

    private void GenerateFanShape()
    {
        // 새 Mesh 생성
        Mesh mesh = new Mesh();

        // 정점(Vertex) 배열 생성
        Vector3[] vertices = new Vector3[segments + 2];
        vertices[0] = Vector3.zero; // 중심점

        float angleStep = angle / segments;
        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = Mathf.Deg2Rad * (-angle / 2 + i * angleStep);
            float x = Mathf.Cos(currentAngle) * radius;
            float y = Mathf.Sin(currentAngle) * radius;
            vertices[i + 1] = new Vector3(x, y, 0);
        }

        // 회전값을 위한 Quaternion 생성 (x=-90, z=-90)
        Quaternion rotation = Quaternion.Euler(-90f, 0f, -90f);

        // 정점 회전 적용
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = rotation * vertices[i]; // 회전 변환 적용
        }

        // 삼각형(Triangle) 배열 생성
        int[] triangles = new int[segments * 3];
        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;         // 중심점
            triangles[i * 3 + 1] = i + 1; // 현재 정점
            triangles[i * 3 + 2] = i + 2; // 다음 정점
        }

        // UV 배열 생성 (텍스처용)
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x / radius + 0.5f, vertices[i].y / radius + 0.5f);
        }

        // Mesh에 데이터 할당
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        // Mesh를 Recalculate
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // MeshFilter에 할당
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        // MeshRenderer 기본 Material 설정
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = material; // 기본 재질
        meshRenderer.enabled = false;
    }



    public void DetectTargets()
    {
        // Collider[]로 범위 내의 모든 오브젝트를 탐지
        Collider[] hits = Physics.OverlapSphere(transform.position, radius * 2, layerMask);

        foreach (Collider hit in hits)
        {
            // 부채꼴 범위 내의 오브젝트인지 확인
            Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget <= angle / 2)
            {
                // 플레이어와 탐지 시스템 사이에 장애물이 있는지 확인
                RaycastHit raycastHit;
                Vector3 direction = hit.transform.position - transform.position;  // 플레이어 방향

                // Raycast로 장애물 감지
                if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), direction, out raycastHit, direction.magnitude))
                {
                    // 장애물이 플레이어가 아니고, 레이어가 'Wall'인 경우 걸러짐
                    if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                    {
                        // 벽이 플레이어와 레이캐스트 사이에 있으면 탐지하지 않음
                        Debug.Log("벽이 가려서 탐지되지 않음: " + raycastHit.collider.name);
                        continue;  // 목표물이 벽 뒤에 있으면 탐지하지 않음
                    }
                }

                // 부채꼴 범위 안에 있고 장애물이 없다면 대상 처리
                Debug.Log($"탐지됨: {hit.name}");
            }
        }
    }


    private void OnDrawGizmos()
    {
        // 기즈모로 Raycast 경로를 표시
        Collider[] hits = Physics.OverlapSphere(transform.position, radius * 2, layerMask);

        foreach (Collider hit in hits)
        {
            Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget <= angle / 2)
            {
                RaycastHit raycastHit;
                Vector3 direction = hit.transform.position - transform.position;

                // Raycast로 장애물 감지
                if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), direction, out raycastHit, direction.magnitude))
                {
                    // 벽이 있으면 빨간색, 아니면 초록색으로 표시
                    if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                    {
                        Gizmos.color = Color.red;  // 벽에 막히면 빨간색
                    }
                    else
                    {
                        Gizmos.color = Color.green;  // 플레이어를 정상적으로 만나면 초록색
                    }

                    // Raycast 경로 그리기
                    Gizmos.DrawLine(transform.position, raycastHit.point);
                }
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f); // 반투명 주황색

    //    // 탐지 범위의 원형을 표시
    //    Gizmos.DrawWireSphere(transform.position, radius * 2);

    //    // 부채꼴을 시각적으로 표시
    //    Vector3 startAngle = Quaternion.Euler(0, -angle / 2, 0) * transform.forward * radius * 2;
    //    Vector3 endAngle = Quaternion.Euler(0, angle / 2, 0) * transform.forward * radius * 2;

    //    Gizmos.DrawLine(transform.position, transform.position + startAngle); // 부채꼴의 시작선
    //    Gizmos.DrawLine(transform.position, transform.position + endAngle);   // 부채꼴의 끝선

    //    // 부채꼴 내부를 점으로 채우기
    //    for (int i = 0; i <= segments; i++)
    //    {
    //        float segmentAngle = -angle / 2 + (angle / segments) * i;
    //        Vector3 segmentDirection = Quaternion.Euler(0, segmentAngle, 0) * transform.forward * radius * 2;
    //        Gizmos.DrawLine(transform.position, transform.position + segmentDirection);
    //    }
    //}
}
