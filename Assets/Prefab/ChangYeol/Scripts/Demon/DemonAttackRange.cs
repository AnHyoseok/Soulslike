using UnityEngine;

namespace BS.Demon
{
public class DemonAttackRange : MonoBehaviour
    {
        public float growSpeed = 2f; // ���� �ӵ�
        private bool isGrowing = false; // ���� ������ Ȯ��

        private Vector3 originalScale; // �ʱ� ũ��
        private Vector3 targetScale; // �ʱ� ũ��

        private void Start()
        {
            // ���� ũ�⸦ �ʱ� ũ��� ����
            originalScale = transform.localScale;
        }

        private void Update()
        {
            UpdateScale();
        }

        public void StartGrowing(Vector3 StartScale, float Range)
        {
            // ��ǥ ũ�� ����
            targetScale = StartScale * Range;
            // ���� ����
            isGrowing = true;
        }
        private void UpdateScale()
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
}