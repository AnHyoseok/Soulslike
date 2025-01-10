using System.Collections;
using UnityEngine;

namespace BS.Demon
{
    public class BallRise : MonoBehaviour
    {
        #region Variables
        public float targetHeight = 15f; // ��ǥ ����
        public float riseSpeed = 2f; // ��� �ӵ�
        private Vector3 startPosition; // ���� ��ġ
        private Vector3 targetPosition; // ��ǥ ��ġ
        private bool isRising = false; // ��� ���� �÷���
        public DemonPattern pattern;
        #endregion
        private void Start()
        {
            // ���� ��ġ�� �������� ��ǥ ��ġ ���
            startPosition = transform.position;
            targetPosition = startPosition + Vector3.up * targetHeight;
        }
        private void Update()
        {
            StartCoroutine(UpRise());
        }

        public void StartRise()
        {
            // ��� ����
            isRising = true;
        }
        IEnumerator UpRise()
        {
            if (isRising)
            {
                // �ε巴�� ���
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, riseSpeed * Time.deltaTime);

                // ��ǥ ��ġ�� �����ߴ��� Ȯ��
                if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
                {
                    isRising = false; // ��� ����
                    yield return new WaitForSeconds(1f);
                    GameObject effgo = Instantiate(pattern.effect[0], transform.position, Quaternion.identity);
                    Destroy(this.gameObject);
                    Destroy(effgo,1.5f);
                }
            }
        }
    }
}