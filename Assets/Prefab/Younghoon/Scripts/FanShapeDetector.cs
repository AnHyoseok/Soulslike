using System.Collections.Generic;
using UnityEngine;

public class FanShapeDetector : MonoBehaviour
{
    public float radius = 5f;       // ��ä�� ������
    public float angle = 90f;      // ��ä�� ����
    public int segments = 20;      // ����� ���� ���׸�Ʈ ����
    public LayerMask layerMask;    // Ž���� ���̾� ����
    //public string targetTag = "Player"; // Ž���� �±�

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
            // Player �±װ� �ƴ� ������Ʈ�� ����
            //if (hit.tag != targetTag) continue;

            // ��ä�� ���� ���� ������Ʈ���� Ȯ��
            Vector3 directionToTarget = (hit.transform.position - position).normalized;
            float angleToTarget = Vector3.Angle(forward, directionToTarget);

            if (angleToTarget <= angle / 2)
            {
                // ��ä�� ���� �ȿ� �ִ� ��� ó��
                Debug.Log($"Ž����: {hit.name}");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f); // ������ ��Ȳ��

        // Ž�� ������ ������ ǥ��
        Gizmos.DrawWireSphere(position, radius);

        // ��ä���� �ð������� ǥ��
        Vector3 startAngle = Quaternion.Euler(0, -angle / 2, 0) * forward * radius;
        Vector3 endAngle = Quaternion.Euler(0, angle / 2, 0) * forward * radius;

        Gizmos.DrawLine(position, position + startAngle); // ��ä���� ���ۼ�
        Gizmos.DrawLine(position, position + endAngle);   // ��ä���� ����

        // ��ä�� ���θ� ������ ä���
        for (int i = 0; i <= segments; i++)
        {
            float segmentAngle = -angle / 2 + (angle / segments) * i;
            Vector3 segmentDirection = Quaternion.Euler(0, segmentAngle, 0) * forward * radius;
            Gizmos.DrawLine(position, position + segmentDirection);
        }
    }
}
