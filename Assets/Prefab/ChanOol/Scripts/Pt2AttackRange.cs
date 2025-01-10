using UnityEngine;

public class Pt2AttackRange : MonoBehaviour
{
    public float growSpeed = 2f; // ���� �ӵ�
    private bool isGrowing = false; // ���� ������ Ȯ��

    private Vector3 originalScale; // �ʱ� ũ��
    private Vector3 targetScale; // �ʱ� ũ��

    private void Start()
    {
        // ���� ũ�⸦ �ʱ� ũ��� ����
        originalScale = transform.localScale;

        StartGrowing(transform.localScale, 150f);
    }

    private void Update()
    {
        UpdateScale();

        // �� Ŀ���� �����ð� �� ������Ʈ ����
        if (isGrowing == false)
        {
            Destroy(gameObject, 2f);
        }
    }

    public void StartGrowing(Vector3 StartScale, float Range)
    {
        // ��ǥ ũ�� ����
        //targetScale = StartScale * Range;
        targetScale = new Vector3(StartScale.x * Range, StartScale.y, StartScale.z * Range);
        // ���� ����
        isGrowing = true;
    }
    public void UpdateScale()
    {
        if (isGrowing)
        {
            // �ε巴�� ũ�� ����
            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, growSpeed * Time.deltaTime);

            // ��ǥ ũ�⿡ �����ߴ��� Ȯ��
            if (Vector3.Distance(transform.localScale, targetScale) < 0.01f)
            {
                isGrowing = false; // ���� ����
                Debug.Log("Reached target size!");
            }
        }
    }
}
