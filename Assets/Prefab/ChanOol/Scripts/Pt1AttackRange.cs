using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]

public class Pt1AttackRange : MonoBehaviour
{
    public float degree = 180;
    public float intervalDegree = 5;
    public float beginOffsetDegree = 0;
    public float radius = 10;

    // 추가: 스케일 설정
    public Vector3 startScale = new Vector3(1f, 1f, 1f); // 초기 스케일
    public Vector3 targetScale = new Vector3(2f, 2f, 2f); // 목표 스케일
    public float scaleSpeed = 1f;  // 스케일 변경 속도

    Mesh mesh;
    MeshFilter meshFilter;

    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;

    int i;
    float beginDegree;
    float endDegree;
    float beginRadian;
    float endRadian;
    float uvRadius = 0.5f;
    Vector2 uvCenter = new Vector2(0.5f, 0.5f);
    float currentIntervalDegree = 0;
    float limitDegree;
    int count;
    int lastCount;

    float beginCos;
    float beginSin;
    float endCos;
    float endSin;

    int beginNumber;
    int endNumber;
    int triangleNumber;

    // Use this for initialization 
    void Start()
    {
        mesh = new Mesh();
        meshFilter = (MeshFilter)GetComponent("MeshFilter");
    }

    // Update is called once per frame 
    // 목표 스케일에 도달한 후, 다시 초기화할 수 있도록 리셋 기능 추가
    void Update()
    {
        currentIntervalDegree = Mathf.Abs(intervalDegree);

        count = (int)(Mathf.Abs(degree) / currentIntervalDegree);
        if (degree % intervalDegree != 0)
        {
            ++count;
        }
        if (degree < 0)
        {
            currentIntervalDegree = -currentIntervalDegree;
        }

        if (lastCount != count)
        {
            mesh.Clear();
            vertices = new Vector3[count * 2 + 1];
            triangles = new int[count * 3];
            uvs = new Vector2[count * 2 + 1];
            vertices[0] = Vector3.zero;
            uvs[0] = uvCenter;
            lastCount = count;
        }

        i = 0;
        beginDegree = beginOffsetDegree;
        limitDegree = degree + beginOffsetDegree;

        while (i < count)
        {
            endDegree = beginDegree + currentIntervalDegree;

            if (degree > 0)
            {
                if (endDegree > limitDegree)
                {
                    endDegree = limitDegree;
                }
            }
            else
            {
                if (endDegree < limitDegree)
                {
                    endDegree = limitDegree;
                }
            }

            beginRadian = Mathf.Deg2Rad * beginDegree;
            endRadian = Mathf.Deg2Rad * endDegree;

            beginCos = Mathf.Cos(beginRadian);
            beginSin = Mathf.Sin(beginRadian);
            endCos = Mathf.Cos(endRadian);
            endSin = Mathf.Sin(endRadian);

            beginNumber = i * 2 + 1;
            endNumber = i * 2 + 2;
            triangleNumber = i * 3;

            vertices[beginNumber].x = beginCos * radius;
            vertices[beginNumber].y = 0;
            vertices[beginNumber].z = beginSin * radius;
            vertices[endNumber].x = endCos * radius;
            vertices[endNumber].y = 0;
            vertices[endNumber].z = endSin * radius;

            triangles[triangleNumber] = 0;
            if (degree > 0)
            {
                triangles[triangleNumber + 1] = endNumber;
                triangles[triangleNumber + 2] = beginNumber;
            }
            else
            {
                triangles[triangleNumber + 1] = beginNumber;
                triangles[triangleNumber + 2] = endNumber;
            }

            if (radius > 0)
            {
                uvs[beginNumber].x = beginCos * uvRadius + uvCenter.x;
                uvs[beginNumber].y = beginSin * uvRadius + uvCenter.y;
                uvs[endNumber].x = endCos * uvRadius + uvCenter.x;
                uvs[endNumber].y = endSin * uvRadius + uvCenter.y;
            }
            else
            {
                uvs[beginNumber].x = -beginCos * uvRadius + uvCenter.x;
                uvs[beginNumber].y = -beginSin * uvRadius + uvCenter.y;
                uvs[endNumber].x = -endCos * uvRadius + uvCenter.x;
                uvs[endNumber].y = -endSin * uvRadius + uvCenter.y;
            }

            beginDegree += currentIntervalDegree;
            ++i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.sharedMesh = mesh;
        meshFilter.sharedMesh.name = "CircularSectorMesh";

        // 스케일 점진적으로 증가
        Vector3 currentScale = transform.localScale;
        Vector3 newScale = Vector3.MoveTowards(currentScale, targetScale, scaleSpeed * Time.deltaTime);

        // 목표 스케일에 도달하면 멈추고, 원하는 경우 리셋할 수 있도록 설정
        if (newScale == targetScale)
        {
            newScale = targetScale; // 목표값에 도달했으면 값을 고정
            ResetScale(); // 리셋 함수 호출
        }

        transform.localScale = newScale;
    }

    // 스케일 초기화 함수
    void ResetScale()
    {
        // 초기 스케일 값으로 리셋하거나 새로운 목표값 설정
        targetScale = startScale; // 원하는 값으로 초기화
        transform.localScale = startScale; // 즉시 초기화하려면 이렇게 할 수 있음
    }

}