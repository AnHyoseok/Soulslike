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
        public Transform firePoint; // ������ �߻� ��ġ

        //���ݹ���
        [SerializeField] private GameObject[] attackRangePrefab;
        public Vector3[] attackRangeScale = new Vector3[2];
        public float[] rangeSize = new float[2];

        //�÷��̾� ã��
        public Transform player; // �÷��̾� ����
        public float rotationSpeed = 5f; // ȸ�� �ӵ�
        #endregion
        private void Start()
        {
            controller = GetComponent<DemonController>();
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
                controller.lastAttackTime[0] = Time.time;
            }
        }
        IEnumerator AttackRangeSpawn(int index)
        {
            GameObject Range = Instantiate(attackRangePrefab[0], Points[index].position + new Vector3(0, 0.2f, 0), Quaternion.identity);
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
            GameObject Range = Instantiate(attackRangePrefab[0], ballRange, Quaternion.identity);
            Range.GetComponent<DemonAttackRange>().StartGrowing(attackRangeScale[1], rangeSize[1]);
            Destroy(Range, 1f);
            yield return new WaitForSeconds[1];
        }
        //���� 3
        public void PerformTeleport()
        {
            if (teleportPoints.Length > 1) // �ڷ���Ʈ ������ 2�� �̻��� �� �ߺ� ���� ����
            {
                Transform closestPoint = null;
                float closestDistance = Mathf.Infinity;

                // ��� �ڷ���Ʈ ������ ��ȸ�ϸ� ���� ����� ���� ã��
                foreach (Transform point in teleportPoints)
                {
                    float distance = Vector3.Distance(player.position, point.position);

                    // �÷��̾���� �Ÿ� �� �� ���� ��ġ�� �ߺ� ����
                    if (distance < closestDistance && point.position != transform.position)
                    {
                        closestDistance = distance;
                        closestPoint = point;
                    }
                }

                if (closestPoint != null)
                {
                    transform.LookAt(player.position);
                    transform.position = closestPoint.position; // ���� ����� ��ġ�� �ڷ���Ʈ
                    // �ڷ���Ʈ ȿ�� ����
                    GameObject effectgo = Instantiate(effect[2], transform.position, Quaternion.identity);
                    Destroy(effectgo, 1f);
                    transform.LookAt(player.position);
                    GameObject Range = Instantiate(attackRangePrefab[1], this.transform.position, Quaternion.identity);
                }
            }
        }
        public void ShootAttack()
        {
            transform.LookAt(player.position);
            controller.lastAttackTime[2] = Time.time;
        }
        //�ٰŸ� ����
        public void CloseAttack()
        {
            transform.LookAt(player.position);
        }
    }
}
