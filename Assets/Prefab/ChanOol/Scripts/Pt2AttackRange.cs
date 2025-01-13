using UnityEngine;

public class Pt2AttackRange : MonoBehaviour
{
    public float growSpeed = 2f; // ���� �ӵ�
    private bool isGrowing = false; // ���� ������ Ȯ��

    private Vector3 originalScale; // �ʱ� ũ��
    private Vector3 targetScale; // �ʱ� ũ��

    // ���̵� ȿ��
    private Material material;
    [SerializeField] private float startAlpha = 1f; // ���� ���� ��
    [SerializeField] private float targetAlpha = 0f; // ��ǥ ���� ��
    [SerializeField] private float fadeSpeed = 0.5f; // �ʴ� ���� �ӵ�
    private bool isFading = true; // ���� �� ��ȯ Ȱ��ȭ ����

    private void Start()
    {
        // ���� ũ�⸦ �ʱ� ũ��� ����
        originalScale = transform.localScale;

        StartGrowing(transform.localScale, 150f);

        // ���̵� ȿ��
        material = GetComponent<MeshRenderer>().material;
        Color color = material.color;
        color.a = startAlpha;
        material.color = color;
    }

    private void Update()
    {
        UpdateScale();

        // �� Ŀ���� �����ð� �� ������Ʈ ����
        if (isGrowing == false)
        {
            // ���̵� ȿ��
            if (isFading)
            {
                // ���� �� ��ȯ ����
                Color color = material.color;
                color.a = Mathf.MoveTowards(color.a, targetAlpha, fadeSpeed * Time.deltaTime);
                material.color = color;

                // ��ǥ ���� ���� �����ϸ� ��ȯ ����
                if (Mathf.Approximately(color.a, targetAlpha))
                {
                    isFading = false;
                }
            }

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
