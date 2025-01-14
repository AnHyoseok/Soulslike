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

        //패턴 1
        public BallRise ball;
        public Transform[] Points;
        public int minSpawnCount = 4; // 최소 생성 개수
        public int maxSpawnCount = 6; // 최대 생성 개수

        //패턴 2
        public Transform ballTranfrom;
        public GameObject ballInstantiate;

        //패턴 3
        public Transform[] teleportPoints; // 텔레포트 가능한 지점들
        public Transform firePoint; // 레이저 발사 위치

        //공격범위
        [SerializeField] private GameObject[] attackRangePrefab;
        public Vector3[] attackRangeScale = new Vector3[2];
        public float[] rangeSize = new float[2];

        //플레이어 찾기
        public Transform player; // 플레이어 참조
        public float rotationSpeed = 5f; // 회전 속도
        #endregion
        private void Start()
        {
            controller = GetComponent<DemonController>();
        }
        //패턴 1
        public void SpawnObjects()
        {
            if (Points.Length == 0 || ball == null)
            {
                Debug.LogWarning("Spawn points or object to spawn not set!");
                return;
            }

            int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1); // 2 또는 3개 생성
            HashSet<int> selectedIndices = new HashSet<int>(); //중복방지
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
            Destroy(Range, 2f);
            yield return new WaitForSeconds(2);
        }
        //패턴 2
        public void AttackBall()
        {
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
            yield return new WaitForSeconds(1);
        }
        //패턴 3
        public void PerformTeleport()
        {
            if (teleportPoints.Length > 1) // 텔레포트 지점이 2개 이상일 때 중복 방지 가능
            {
                Transform closestPoint = null;
                float closestDistance = Mathf.Infinity;

                // 모든 텔레포트 지점을 순회하며 가장 가까운 지점 찾기
                foreach (Transform point in teleportPoints)
                {
                    float distance = Vector3.Distance(player.position, point.position);

                    // 플레이어와의 거리 비교 및 현재 위치와 중복 방지
                    if (distance < closestDistance && point.position != transform.position)
                    {
                        closestDistance = distance;
                        closestPoint = point;
                    }
                }

                if (closestPoint != null)
                {
                    transform.position = closestPoint.position; // 가장 가까운 위치로 텔레포트
                    transform.LookAt(player.position);
                    // 텔레포트 효과 생성
                    GameObject effectgo = Instantiate(effect[2], transform.position, Quaternion.identity);
                    Destroy(effectgo, 1f);
                }
            }
        }
        public void ShootAttack()
        {
            controller.lastAttackTime[2] = Time.time;
        }
        //근거리 공격
        public void CloseAttack()
        {
            transform.LookAt(player.position);
        }
    }
}
