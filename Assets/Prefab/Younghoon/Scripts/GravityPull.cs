using UnityEngine;

namespace BS.Enemy.Set
{
    public class GravityPull : MonoBehaviour
    {
        #region Variables
        public float pullForce = 10f; // ������� ���� ũ��
        public float maxDistance; // �ִ� ������ �Ÿ�

        private SphereCollider sphereCollider;
        private LayerMask playerLayer;  //�÷��̾� ���̾� ����ũ�� �Ҵ��� ����
        #endregion

        private void Start()
        {
            sphereCollider = GetComponent<SphereCollider>();
            // ���̾��ũ�� �̸����� �����ͼ� ��Ʈ ����: "���� ����Ʈ" �� ����Ѵ�
            // 6 -> 0000 0100 0000(Total : 256) �̷������� ���̾��ũ�� �Ҵ�Ȱ��� ã�� ��ȯ��Ŵ
            playerLayer = 1 << LayerMask.NameToLayer(SetProperty.PLAYER_LAYER);
            maxDistance = sphereCollider.radius * Mathf.Max(transform.root.localScale.x, transform.root.localScale.y, transform.root.localScale.z);
        }

        private void FixedUpdate()
        {
            // Physics.OverlapSphere�� ����� sphereCollider.radius ���� "Player" ���̾ �˻�
            Collider[] colliders = Physics.OverlapSphere(transform.position, maxDistance, playerLayer);

            foreach (Collider collider in colliders)
            {
                Transform target = collider.transform;
                if (target != null)
                {
                    // ���� ���� ��� (�߽��� - ������Ʈ ��ġ)
                    Vector3 direction = transform.position - target.position;

                    // y ���� �����ϰ� x�� z ���� ���
                    direction.y = 0;

                    // �Ÿ� ��� (���� �Ÿ���)
                    float distance = direction.magnitude;

                    // �Ÿ��� ���� �� ���� (���� ����)
                    float speed = Mathf.Lerp(pullForce, 0, distance / maxDistance);

                    // �� ���� (x, z �ุ)
                    target.position += direction.normalized * speed * Time.deltaTime;

                    //TODO : �������� ������ �ڵ带 �����ϱ�

#if UNITY_EDITOR
                    Debug.Log($"�Ÿ�: {distance}, ���: {collider.gameObject.name}");
                    Debug.Log($"�ӵ�: {speed}, ����: {direction.normalized}");
#endif

                }
            }
        }

        #region �׽�Ʈ ����Ȯ�ο� �����
        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawWireSphere(transform.position, maxDistance);
        //}
        #endregion
    }
}