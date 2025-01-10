using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UIElements;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FanShape : MonoBehaviour
{
    public float radius = 1f;         // ��ä���� ������
    public float angle = 90f;        // ��ä���� ����
    public int segments = 10;        // ��ä���� �̷�� ���׸�Ʈ(�ﰢ�� ��)
    public Material material;
    public LayerMask layerMask;    // Ž���� ���̾� ����

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
        // �� Mesh ����
        Mesh mesh = new Mesh();

        // ����(Vertex) �迭 ����
        Vector3[] vertices = new Vector3[segments + 2];
        vertices[0] = Vector3.zero; // �߽���

        float angleStep = angle / segments;
        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = Mathf.Deg2Rad * (-angle / 2 + i * angleStep);
            float x = Mathf.Cos(currentAngle) * radius;
            float y = Mathf.Sin(currentAngle) * radius;
            vertices[i + 1] = new Vector3(x, y, 0);
        }

        // ȸ������ ���� Quaternion ���� (x=-90, z=-90)
        Quaternion rotation = Quaternion.Euler(-90f, 0f, -90f);

        // ���� ȸ�� ����
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = rotation * vertices[i]; // ȸ�� ��ȯ ����
        }

        // �ﰢ��(Triangle) �迭 ����
        int[] triangles = new int[segments * 3];
        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;         // �߽���
            triangles[i * 3 + 1] = i + 1; // ���� ����
            triangles[i * 3 + 2] = i + 2; // ���� ����
        }

        // UV �迭 ���� (�ؽ�ó��)
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x / radius + 0.5f, vertices[i].y / radius + 0.5f);
        }

        // Mesh�� ������ �Ҵ�
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        // Mesh�� Recalculate
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // MeshFilter�� �Ҵ�
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        // MeshRenderer �⺻ Material ����
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = material; // �⺻ ����
        meshRenderer.enabled = false;
    }



    public void DetectTargets()
    {
        // Collider[]�� ���� ���� ��� ������Ʈ�� Ž��
        Collider[] hits = Physics.OverlapSphere(transform.position, radius * 2, layerMask);

        foreach (Collider hit in hits)
        {
            // ��ä�� ���� ���� ������Ʈ���� Ȯ��
            Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget <= angle / 2)
            {
                // �÷��̾�� Ž�� �ý��� ���̿� ��ֹ��� �ִ��� Ȯ��
                RaycastHit raycastHit;
                Vector3 direction = hit.transform.position - transform.position;  // �÷��̾� ����

                // Raycast�� ��ֹ� ����
                if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), direction, out raycastHit, direction.magnitude))
                {
                    // ��ֹ��� �÷��̾ �ƴϰ�, ���̾ 'Wall'�� ��� �ɷ���
                    if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                    {
                        // ���� �÷��̾�� ����ĳ��Ʈ ���̿� ������ Ž������ ����
                        Debug.Log("���� ������ Ž������ ����: " + raycastHit.collider.name);
                        continue;  // ��ǥ���� �� �ڿ� ������ Ž������ ����
                    }
                }

                // ��ä�� ���� �ȿ� �ְ� ��ֹ��� ���ٸ� ��� ó��
                Debug.Log($"Ž����: {hit.name}");
            }
        }
    }


    private void OnDrawGizmos()
    {
        // ������ Raycast ��θ� ǥ��
        Collider[] hits = Physics.OverlapSphere(transform.position, radius * 2, layerMask);

        foreach (Collider hit in hits)
        {
            Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget <= angle / 2)
            {
                RaycastHit raycastHit;
                Vector3 direction = hit.transform.position - transform.position;

                // Raycast�� ��ֹ� ����
                if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), direction, out raycastHit, direction.magnitude))
                {
                    // ���� ������ ������, �ƴϸ� �ʷϻ����� ǥ��
                    if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                    {
                        Gizmos.color = Color.red;  // ���� ������ ������
                    }
                    else
                    {
                        Gizmos.color = Color.green;  // �÷��̾ ���������� ������ �ʷϻ�
                    }

                    // Raycast ��� �׸���
                    Gizmos.DrawLine(transform.position, raycastHit.point);
                }
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f); // ������ ��Ȳ��

    //    // Ž�� ������ ������ ǥ��
    //    Gizmos.DrawWireSphere(transform.position, radius * 2);

    //    // ��ä���� �ð������� ǥ��
    //    Vector3 startAngle = Quaternion.Euler(0, -angle / 2, 0) * transform.forward * radius * 2;
    //    Vector3 endAngle = Quaternion.Euler(0, angle / 2, 0) * transform.forward * radius * 2;

    //    Gizmos.DrawLine(transform.position, transform.position + startAngle); // ��ä���� ���ۼ�
    //    Gizmos.DrawLine(transform.position, transform.position + endAngle);   // ��ä���� ����

    //    // ��ä�� ���θ� ������ ä���
    //    for (int i = 0; i <= segments; i++)
    //    {
    //        float segmentAngle = -angle / 2 + (angle / segments) * i;
    //        Vector3 segmentDirection = Quaternion.Euler(0, segmentAngle, 0) * transform.forward * radius * 2;
    //        Gizmos.DrawLine(transform.position, transform.position + segmentDirection);
    //    }
    //}
}
