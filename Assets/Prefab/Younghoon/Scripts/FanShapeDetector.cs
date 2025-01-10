using System.Collections.Generic;
using UnityEngine;

public class FanShapeDetector : MonoBehaviour
{
    public float radius = 5f;       // 부채꼴 반지름
    public float angle = 90f;      // 부채꼴 각도
    public int segments = 20;      // 기즈모를 위한 세그먼트 개수
    public LayerMask layerMask;    // 탐지할 레이어 설정
    //public string targetTag = "Player"; // 탐지할 태그

    private Vector3 position => transform.position;
    private Vector3 forward => transform.forward;

    void Update()
    {
        DetectTargets();
    }

    private void DetectTargets()
    {
        Collider[] hits = Physics.OverlapSphere(position, radius, layerMask);

        foreach (Collider hit in hits)
        {
            // Player 태그가 아닌 오브젝트는 무시
            //if (hit.tag != targetTag) continue;

            // 부채꼴 범위 내의 오브젝트인지 확인
            Vector3 directionToTarget = (hit.transform.position - position).normalized;
            float angleToTarget = Vector3.Angle(forward, directionToTarget);

            if (angleToTarget <= angle / 2)
            {
                // 부채꼴 범위 안에 있는 대상 처리
                Debug.Log($"탐지됨: {hit.name}");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f); // 반투명 주황색

        // 탐지 범위의 원형을 표시
        Gizmos.DrawWireSphere(position, radius);

        // 부채꼴을 시각적으로 표시
        Vector3 startAngle = Quaternion.Euler(0, -angle / 2, 0) * forward * radius;
        Vector3 endAngle = Quaternion.Euler(0, angle / 2, 0) * forward * radius;

        Gizmos.DrawLine(position, position + startAngle); // 부채꼴의 시작선
        Gizmos.DrawLine(position, position + endAngle);   // 부채꼴의 끝선

        // 부채꼴 내부를 점으로 채우기
        for (int i = 0; i <= segments; i++)
        {
            float segmentAngle = -angle / 2 + (angle / segments) * i;
            Vector3 segmentDirection = Quaternion.Euler(0, segmentAngle, 0) * forward * radius;
            Gizmos.DrawLine(position, position + segmentDirection);
        }
    }
}
