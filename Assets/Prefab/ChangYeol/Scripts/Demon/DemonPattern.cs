using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BS.Demon
{
    public class DemonPattern : MonoBehaviour
    {
        #region Variables
        private DemonController controller;
        public GameObject[] effect;

        //���� 1
        public BallRise ball;
        public Transform[] Points;
        public int minSpawnCount = 4; // �ּ� ���� ����
        public int maxSpawnCount = 6; // �ִ� ���� ����

        //���� 2
        public Transform ballTranfrom;
        public GameObject ballInstantiate;

        //���� 3
        public Transform[] teleportPoints; // �ڷ���Ʈ ������ ������
        public LineRenderer lineRenderer; // ������ �ð�ȭ�� ���� LineRenderer
        public Transform firePoint; // ������ �߻� ��ġ
        public float laserDistance = 50f; // ������ �ִ� ��Ÿ�
        public float laserDuration = 0.1f; // ������ ǥ�� �ð�
        public LayerMask targetLayer; // �������� �浹�� ���̾� ����

        //���ݹ���
        [SerializeField] private GameObject attackRangePrefab;
        public Vector3[] attackRangeScale = new Vector3[2];
        public float[] rangeSize = new float[2];

        //�÷��̾� ã��
        public Transform player; // �÷��̾� ����
        public float rotationSpeed = 5f; // ȸ�� �ӵ�
        #endregion
        private void Start()
        {
            controller = GetComponent<DemonController>();

            lineRenderer.positionCount = 2; // ���۰� �� �� ���� ����Ʈ
            lineRenderer.enabled = false; // �⺻������ ��Ȱ��ȭ
        }
        //���� 1
        public void SpawnObjects()
        {
            transform.LookAt(player.position);
            if (Points.Length == 0 || ball == null)
            {
                Debug.LogWarning("Spawn points or object to spawn not set!");
                return;
            }

            int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1); // 2 �Ǵ� 3�� ����
            HashSet<int> selectedIndices = new HashSet<int>(); //�ߺ�����
            while (selectedIndices.Count < spawnCount)
            {
                int randomIndex = Random.Range(0, Points.Length);
                selectedIndices.Add(randomIndex);
            }

            foreach (int index in selectedIndices)
            {
                StartCoroutine(AttackRangeSpawn(index));
                GameObject game = Instantiate(ball.gameObject, Points[index].position, Quaternion.identity);
                game.GetComponent<BallRise>().StartRise();
                Debug.Log($"Spawned object at point {index + 1}");
                controller.lastAttackTime[0] = Time.time;
            }
        }
        IEnumerator AttackRangeSpawn(int index)
        {
            GameObject Range = Instantiate(attackRangePrefab, Points[index].position + new Vector3(0, 0.2f, 0), Quaternion.identity);
            Range.GetComponent<DemonAttackRange>().StartGrowing(attackRangeScale[0], rangeSize[0]);
            yield return new WaitForSeconds[3];
            Destroy(Range, 2f);
        }
        //���� 2
        public void AttackBall()
        {
            transform.LookAt(player.position);
            GameObject attackball = Instantiate(ballInstantiate, ballTranfrom.position, Quaternion.identity);
            StartCoroutine(AttackRangeBall());
            GameObject effgo = Instantiate(effect[1], attackball.transform.position, Quaternion.identity);
            if (effgo != null && attackball != null)
            {
                Destroy(attackball, 1f);
                Destroy(effgo, 2f);
            }
            controller.lastAttackTime[1] = Time.time;
        }
        IEnumerator AttackRangeBall()
        {
            Vector3 ballRange = new Vector3(ballTranfrom.position.x, 0.2f, ballTranfrom.position.z);
            GameObject Range = Instantiate(attackRangePrefab, ballRange, Quaternion.identity);
            Range.GetComponent<DemonAttackRange>().StartGrowing(attackRangeScale[1], rangeSize[1]);
            yield return new WaitForSeconds[3];
            Destroy(Range, 2f);
        }
        //���� 3
        public void PerformTeleport()
        {
            transform.LookAt(player.position);
            if (teleportPoints.Length > 1) // �ڷ���Ʈ ������ 2�� �̻��� ���� �ߺ� ���� ����
            {
                int randomIndex;
                do
                {
                    randomIndex = Random.Range(0, teleportPoints.Length);
                }
                while (transform.position == teleportPoints[randomIndex].position); // ���� ��ġ�� �ٸ� ��ġ ����

                // �ڷ���Ʈ ȿ�� ����
                GameObject effgo = Instantiate(effect[2], transform.position, Quaternion.identity);
                Destroy(effgo, 1f);

                // �ڷ���Ʈ ��ġ ����
                transform.position = teleportPoints[randomIndex].position;

                // �ڷ���Ʈ ȿ�� ����
                GameObject effectgo = Instantiate(effect[2], transform.position, Quaternion.identity);
                Destroy(effectgo, 1f);
            }
        }
        public void ShootAttack()
        {
            transform.LookAt(player.position);
            controller.lastAttackTime[2] = Time.time;
        }
        private IEnumerator ShowLaser(Vector3 start, Vector3 end)
        {
            lineRenderer.SetPosition(0, start); // ���� ��ġ
            lineRenderer.SetPosition(1, end);   // �� ��ġ
            lineRenderer.enabled = true;

            yield return new WaitForSeconds(laserDuration); // ������ ǥ�� �ð�

            lineRenderer.enabled = false; // ������ ��Ȱ��ȭ
        }
        public void FireLaser()
        {
            // ������ �߻� ���� ����
            Vector3 start = firePoint.position;
            Vector3 direction = firePoint.forward;
            Vector3 end = start + direction * laserDistance;

            // ������ �浹 ����
            RaycastHit hit;
            if (Physics.Raycast(start, direction, out hit, laserDistance, targetLayer))
            {
                end = hit.point; // �浹 �������� �� ��ġ ������Ʈ
                Debug.Log($"Laser hit {hit.collider.name}");
            }
            // LineRenderer�� ����Ͽ� ������ ǥ��
            StartCoroutine(ShowLaser(start, end));
        }
    }
}
