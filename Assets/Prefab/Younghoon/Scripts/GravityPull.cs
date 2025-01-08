using UnityEngine;

public class GravityPull : MonoBehaviour
{
    public float pullForce = 10f; // ������� ���� ũ��
    public float maxDistance = 5f; // �ִ� ������ �Ÿ�

    private void FixedUpdate()
    {
        // Physics.OverlapSphere�� ����� maxDistance ���� ��� �ݶ��̴� �˻�
        Collider[] colliders = Physics.OverlapSphere(transform.position, maxDistance);

        #region �÷��̾� ���̾� �߰��� �ڵ� ��ü
        // Physics.OverlapSphere�� ����� maxDistance ���� "Player" ���̾ �˻�
        //Collider[] colliders = Physics.OverlapSphere(transform.position, maxDistance, playerLayerMask);

        //foreach (Collider collider in colliders)
        //{
        //    Transform target = collider.transform;

        //    if (target != null)
        //    {
        //        // ���� ���� ��� (�߽��� - ������Ʈ ��ġ)
        //        Vector3 direction = transform.position - target.position;

        //        // y ���� �����ϰ� x�� z ���� ���
        //        direction.y = 0;

        //        // �Ÿ� ��� (���� �Ÿ���)
        //        float distance = direction.magnitude;

        //        // �Ÿ��� ���� �� ���� (���� ����)
        //        float speed = Mathf.Lerp(pullForce, 0, distance / maxDistance);

        //        // �� ���� (x, z �ุ)
        //        target.position += direction.normalized * speed * Time.deltaTime;
        //    }
        //}
        #endregion

        #region ���̾� �߰��� ����׿� GravityPull ȿ�� (���� ���� �ʿ�)
        foreach (Collider collider in colliders)
        {
            Debug.Log("1");
            if (collider.gameObject.tag != "Player")
            {
                Debug.Log("2");
                continue;
            }
            Debug.Log("3");
            Transform target = collider.transform;
            if (target != null)
            {
                // ���� ���� ��� (�߽��� - ������Ʈ ��ġ)
                Vector3 direction = transform.position - target.position;

                // y ���� �����ϰ� x�� z ���� ���
                direction.y = 0;

                // �Ÿ� ���
                float distance = direction.magnitude;
                Debug.Log($"�Ÿ�: {distance}, ���: {collider.gameObject.name}");
                // �Ÿ��� ���� �� ���� (���� ����)
                float speed = Mathf.Lerp(pullForce, 0, distance / maxDistance);

                // �� ����
                target.position += direction.normalized * speed * Time.deltaTime;
                Debug.Log($"�ӵ�: {speed}, ����: {direction.normalized}");
            }
        }
        #endregion
    }

    #region �׽�Ʈ ����Ȯ�ο� �����
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
    #endregion
}
